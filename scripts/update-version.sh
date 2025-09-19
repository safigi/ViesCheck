#!/bin/bash
# Bash script for Linux/macOS CI/CD

set -e

NEW_VERSION=$1
PRE_RELEASE=${2:-""}

if [ -z "$NEW_VERSION" ]; then
    echo "Usage: $0 <version> [pre-release]"
    echo "Example: $0 1.2.3"
    echo "Example: $0 1.2.3 beta"
    exit 1
fi

echo "🔄 Updating version to $NEW_VERSION"

# Parse version
IFS='.' read -ra VERSION_PARTS <<< "$NEW_VERSION"
if [ ${#VERSION_PARTS[@]} -ne 3 ]; then
    echo "❌ Version must be in format Major.Minor.Patch"
    exit 1
fi

MAJOR=${VERSION_PARTS[0]}
MINOR=${VERSION_PARTS[1]}
PATCH=${VERSION_PARTS[2]}

# Update Directory.Build.props
BUILD_PROPS="$(dirname "$0")/../Directory.Build.props"
if [ -f "$BUILD_PROPS" ]; then
    echo "📝 Updating Directory.Build.props..."
    
    sed -i.bak \
        -e "s/<MajorVersion>[0-9]*<\/MajorVersion>/<MajorVersion>$MAJOR<\/MajorVersion>/" \
        -e "s/<MinorVersion>[0-9]*<\/MinorVersion>/<MinorVersion>$MINOR<\/MinorVersion>/" \
        -e "s/<PatchVersion>[0-9]*<\/PatchVersion>/<PatchVersion>$PATCH<\/PatchVersion>/" \
        -e "s/<PreReleaseLabel>.*<\/PreReleaseLabel>/<PreReleaseLabel>$PRE_RELEASE<\/PreReleaseLabel>/" \
        "$BUILD_PROPS"
    
    rm "$BUILD_PROPS.bak"
    echo "✅ Directory.Build.props updated"
fi

# Create git tag
if [ -n "$PRE_RELEASE" ]; then
    TAG_NAME="v$NEW_VERSION-$PRE_RELEASE"
else
    TAG_NAME="v$NEW_VERSION"
fi

echo "🏷️  Creating git tag: $TAG_NAME"
git tag -a "$TAG_NAME" -m "Release $TAG_NAME"
echo "✅ Git tag created: $TAG_NAME"

echo "🎉 Version update complete!"
echo "Next steps:"
echo "  1. dotnet build"
echo "  2. dotnet pack"
echo "  3. git push origin $TAG_NAME"
