# Fetches the latest commit from Github and applies it for the build

if ($args.Count -lt 1)
{
    echo "Not enough arguments provided!"
    exit 1
}
# Had to do this to bypass MSVC's stupid trailing slash
$projectDir = $args -join " "

if (!(Get-Command -Name "git" -ErrorAction SilentlyContinue))
{
    echo "Git not installed/not in the path!"
    exit 1
}

$programClassPath = "${projectDir}\Client.cs"
$programClass = Get-Content -Path $programClassPath

#$lastCommitHash = $(git ls-remote $repositoryUrl refs/heads/main).Split("`t")[0]
$lastCommitHash = $(git rev-parse HEAD)
echo "The commit hash for this build is ${lastCommitHash}"

# Replace the commit hash in assembly info
$newProgramClass = $programClass -replace '(?<=public const string VERSION_COMMIT = ").*(?=";)', $lastCommitHash
Set-Content -Path $programClassPath -Value $newProgramClass
echo "Successfully changed the commit hash in the build!"