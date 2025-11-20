SimPE is a Community effort and is released under the GPL.

REMEMBER YOU NEED TO INSTALL Microsoft .NET Framework 2.0 IN ORDER TO RUN SimPE. 


SimPE Commandline Arguments:
---------------------------
Display other available Commandline Arguments
-help

Start SimPE with a pre-saved Profile
-profile savedprofilename

To load a specific file at start-up
-load filename

Set the Theme of the new GUI to a more classic/modern/simple like appearance
SimPE.exe -classicpreset
SimPE.exe -modernpreset
SimPE.exe -simplepreset

Create a new Package file from a package Description (package.xml)
SimPE.exe -build -desc [package.xml] -out [output].package

Create a new Texture File from an arbitary Image File
SimPE.exe -txtr -image [imgfile] -out [output].package -name [textureName] -format [dxt1|dxt3|dxt5|raw8|raw24|raw32] -levels [nr] -width [max. Width] -height [max. Height]

Make an old package EP Ready
SimPE.exe -fix -package [input].package -modelname [newModelName] -prefix [newPrefix]

SimPE File Table:
---------------------------
SimPE uses a Filetable to locate packed Files. This is used when creating a clone. SimPE is searching the FileTable for the needed Files.
This table is built from the content of package Files found in special Locations. The Folders where SimPE searches those package Files can be set in the data/folders.xreg (xml File).

Here is the DTD used to process the folders.xreg File
<?xml version="1.0" encoding="UTF-8"?>
<!ELEMENT folders (filetable)>
<!ELEMENT filetable (path*)>
<!ELEMENT path (#PCDATA)>
<!ATTLIST path
	root (game | simpe | folder) recursive IMPLIED
>

if some resources are found in more than one location, SimPE will always choose the resource found in the path that that was listed first


More Help:
---------------------------
Check out The SimPE preservation Project at http://www.modthesims.info/showthread.php?t=630456
Lot's of basic Questions concerning SimPE were already discussed there!
Please report any errors you find in the forum.


Licenses:
---------------------------
Hash Class
Classless.Hasher from http://www.classless.net/, the Code was not modified, so as long as the Source 
of SimPE isn't available, you can download the Classes Source on the location given above.

SharZipLib 
http://www.icsharpcode.net/OpenSource/SharpZipLib/Default.aspx

The New SimPE GUI was enhanced using Controls by 
SteepValley (http://www.steepvalley.net/).

Most of the Icons used in SimPE were created by http://www.everaldo.com/ (KDE Crystal Clear Theme)

SimPE is using the 7-Zip commandline Utility to handle archives (like .7z, .rar...). The Software is distributed under GPL. For more Information visit: http://www.7-zip.org.


Credits:
---------------------------
Finally got the Time to write some quick Credits for some of the People that helped to develop
SimPE.

Basic Package handling Algorithm was programmed after the WIKI entries on MTS II
Interest/Character Properties/Skills were discovered by Buggi
Woman/Man Interests were posted by Makkentass
German Translation by QAlphAlpha
Thanks to http://www.simszone.de/ for offering a mirror for the German Version
And thanks to all people who have tested SimPE in the last weeks and sent me their Bug Reports.
BHAV and BCON Formats are implemented as Described on http://simtech.sourceforge.net/. Thanks 
to Garrett Berg for his great help on the BHAV and TTAB usage!
The SemiGlobal Groups were discovered by Inge Jones.
Thanks to Pumuckl for the (correct) DXT3, DXT5 Decompression Code.
Thanks to Carrigon for the Unblur Patch Line
Thanks to Peter and Inge Jones for the PJSE Plugins
Thanks to Motoki for discovering the new Relationship type values.
Thanks to Pinhead for discovering the new Fields for YA Clothes and Sim Exports for Bodyshop
Thanks to JMPescado for the EP -Family Information Format
Thanks to Pinhead, Numenor and RGiles for finding everything needed to make the Recolor Options in the Object Workshop possible.
Installer by Uecasm
Additional PhotoStudio Templates created by Pinhead and Deedee (http://daydream.etowns.net)
GMDC Obj Exporter created by Delphy (http://www.modthesims2.com)
GMDC and ANIM decoding by Miche
Xanathon for his valuable help
Theo for revisions of some SimPE Plugins like Surgery (http://theos.chewbakkas.net/)

Polish translation by wlodi
Spanish translation by Malakun
Italian translation by Sims2Cri
German proofreading by Xanathon
Russian translation by Necromante
French translation by deeVo
