# Automated Version Synchronization Guide

## ✅ **Problem Solved!**

Your .nuspec and assembly version synchronization is now fully automated! Here are the solutions implemented:

## 🎯 **Current Setup:**

### **1. Centralized Version Management**
- **Single source of truth**: `Directory.Build.props` 
- **All versions sync automatically**: Assembly, File, Package versions
- **No more manual .nuspec updates needed**

### **2. Modern .NET Approach (Recommended)**
```xml
<!-- Directory.Build.props -->
<MajorVersion>1</MajorVersion>
<MinorVersion>0</MinorVersion>
<PatchVersion>2</PatchVersion>
<PreReleaseLabel></PreReleaseLabel>
```

All versions are derived automatically:
- **AssemblyVersion**: 1.0.2
- **FileVersion**: 1.0.2  
- **PackageVersion**: 1.0.2
- **InformationalVersion**: 1.0.2+build.{BuildNumber}

## 🚀 **Automation Options:**

### **Option A: Manual Script Updates**
```powershell
# PowerShell
.\scripts\update-version.ps1 1.0.3

# Bash
./scripts/update-version.sh 1.0.3
```

### **Option B: GitHub Actions (Automatic)**
1. **Push a git tag**: `git tag v1.0.3 && git push origin v1.0.3`
2. **GitHub Actions automatically**:
   - Builds the project
   - Runs tests
   - Creates NuGet package
   - Publishes to NuGet.org
   - Creates GitHub release

### **Option C: Pre-release Versions**
```powershell
.\scripts\update-version.ps1 1.1.0 beta
# Creates: 1.1.0-beta
```

## 📦 **Benefits Achieved:**

✅ **No more .nuspec file** - everything in .csproj  
✅ **Single version source** - Directory.Build.props  
✅ **Automatic sync** - Assembly = File = Package version  
✅ **CI/CD ready** - GitHub Actions workflow  
✅ **Git tags integration** - Releases tied to tags  
✅ **Pre-release support** - Alpha/Beta versions  

## 🔧 **How to Update Versions:**

### **Quick Update (Recommended):**
```bash
# Update patch version
git tag v1.0.3
git push origin v1.0.3
# GitHub Actions handles the rest!
```

### **Manual Update:**
```powershell
# Update Directory.Build.props manually
# Or use the script:
.\scripts\update-version.ps1 1.0.3
```

### **CI/CD Integration:**
The GitHub Actions workflow automatically:
1. Extracts version from git tag
2. Builds with correct version
3. Creates NuGet package
4. Publishes to NuGet.org
5. Creates GitHub release

## 💡 **Example Workflow:**

```bash
# 1. Update version
git tag v1.0.3
git push origin v1.0.3

# 2. GitHub Actions automatically:
#    - Builds ViesApi.1.0.3.nupkg
#    - Publishes to NuGet
#    - Creates GitHub release

# 3. Done! ✅
```

## 🎉 **Version Sync Problem = SOLVED!**

No more manual synchronization needed. All versions stay in perfect sync automatically!
