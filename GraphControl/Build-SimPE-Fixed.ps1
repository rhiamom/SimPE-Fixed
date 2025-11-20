# Set your SimPE source path
$sourceRoot = "C:\Users\rhiam\Documents\SimPESource"
$solutionPath = Join-Path $sourceRoot "SimPE-Fixed.sln"

# Step 1: Create a new solution if it doesn't exist
if (-Not (Test-Path $solutionPath)) {
    Write-Host "🛠 Creating new solution: SimPE-Fixed.sln"
    dotnet new sln -n SimPE-Fixed -o $sourceRoot
} else {
    Write-Host "✔ Solution already exists: SimPE-Fixed.sln"
}

# Step 2: Add all .csproj files found in source tree
Get-ChildItem -Path $sourceRoot -Filter *.csproj -Recurse | ForEach-Object {
    $projectPath = $_.FullName
    Write-Host "➕ Adding project: $projectPath"
    dotnet sln $solutionPath add $projectPath
}

Write-Host "`n✅ All projects added to SimPE-Fixed.sln successfully."