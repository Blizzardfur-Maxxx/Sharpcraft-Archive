# Clears the commit in the project class (to not have to commit everytime you build)

if ($args.Count -lt 1)
{
    echo "Not enough arguments provided!"
    exit 1
}
# Had to do this to bypass MSVC's stupid trailing slash
$projectDir = $args -join " "

$programClassPath = "${projectDir}\Client.cs"
$programClass = Get-Content -Path $programClassPath

# Replace the commit hash in assembly info
$newProgramClass = $programClass -replace '(?<=public const string VERSION_COMMIT = ").*(?=";)', "0000000000000000000000000000000000000000"
Set-Content -Path $programClassPath -Value $newProgramClass
echo "Successfully cleared the commit hash in the build!"