# Strip <OutputPath> overrides that point to shared folders (..\__Release\,
# ..\__Debug\, ..\bin\Debug\, ..\bin\Release\, etc.). After this, MSBuild
# falls back to the per-project default bin\$(Configuration)\, which kills the
# parallel-build race on shared GenerateDepsFile writes.
#
# Directory.Build.targets still aggregates outputs to $(SolutionDir)bin\Release\
# via SimPeCopyToUnifiedBin.

$root = $PSScriptRoot
if (-not $root) { $root = Get-Location }

# Strip ANY <OutputPath> whose value uses ..\ to climb out of the project folder
# (i.e. resolves to a folder shared with other projects). Per-project relative
# paths (no leading ..\) are left alone. Catches all of:
#   ..\__Release\, ..\__Debug\, ..\bin\Debug\, ..\bin\Release\,
#   ..\__Debug\Plugins\, ..\__Release\Plugins\, ..\bin\Debug\Plugins\,
#   ..\..\__Debug\Plugins\, ..\..\__Release\, ..\..\__Release\Plugins\,
#   ..\..\bin\Debug\Plugins\, etc.
$combined = '(?im)^[ \t]*<OutputPath>(?:\.\.\\)+[^<]*</OutputPath>[ \t]*\r?\n'

$csprojs = Get-ChildItem -Path $root -Recurse -Filter *.csproj | Where-Object {
    $content = Get-Content -Raw -LiteralPath $_.FullName
    $content -match '<OutputPath>(?:\.\.\\)+'
}

foreach ($f in $csprojs) {
    $original = Get-Content -Raw -LiteralPath $f.FullName
    $stripped = [regex]::Replace($original, $combined, '')
    if ($stripped -ne $original) {
        Set-Content -LiteralPath $f.FullName -Value $stripped -NoNewline
        Write-Host "Stripped: $($f.FullName.Substring($root.Length + 1))"
    }
}

Write-Host ""
Write-Host "Done. $($csprojs.Count) csproj(s) processed."
