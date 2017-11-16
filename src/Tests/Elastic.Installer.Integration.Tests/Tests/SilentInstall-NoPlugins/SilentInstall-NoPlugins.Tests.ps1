$currentDir = Split-Path -parent $MyInvocation.MyCommand.Path
Set-Location $currentDir

# mapped sync folder for common scripts
. $currentDir\..\common\Utils.ps1
. $currentDir\..\common\CommonTests.ps1
. $currentDir\..\common\SemVer.ps1

Get-Version
Get-PreviousVersions

Describe "Silent Install with no plugins" {

    Invoke-SilentInstall -Exeargs @("PLUGINS=")

    Context-PingNode -XPackSecurityInstalled $false

    Context-PluginsInstalled -Expected @{ Plugins=@() }

    Context-ClusterNameAndNodeName

	Copy-ElasticsearchLogToOut
}

Describe "Silent Uninstall with no plugins" {

    Invoke-SilentUninstall

	Context-NodeNotRunning

	Context-EsConfigEnvironmentVariableNull

	Context-EsHomeEnvironmentVariableNull

	Context-MsiNotRegistered

	Context-ElasticsearchServiceNotInstalled

	$ProgramFiles = Get-ProgramFilesFolder
	$ChildPath = Get-ChildPath
    $ExpectedHomeFolder = Join-Path -Path $ProgramFiles -ChildPath $ChildPath

	Context-EmptyInstallDirectory -Path $ExpectedHomeFolder
}