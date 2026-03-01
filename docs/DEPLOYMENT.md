# 배포 가이드

## 개요

NoxyTools는 두 가지 배포 방식을 지원합니다.

| 방식 | 설명 | 사용 시점 |
|---|---|---|
| **GitHub Actions (자동)** | 태그 푸시 시 CI가 빌드→인스톨러 생성→Release 업로드 자동 수행 | 정식 릴리즈 |
| **로컬 수동 빌드** | 로컬에서 직접 publish 후 Inno Setup으로 인스톨러 생성 | 테스트·긴급 빌드 |

---

## 1. GitHub Actions 자동 배포 (권장)

워크플로 파일: [`.github/workflows/release.yml`](../.github/workflows/release.yml)

`v*` 형식의 태그를 푸시하면 자동으로 실행됩니다.

### 배포 절차

#### 1-1. CHANGELOG.md 업데이트

[`CHANGELOG.md`](../CHANGELOG.md)의 `## [Unreleased]` 섹션을 새 버전으로 이동합니다.

```markdown
## [0.2.0] - 2026-04-01

### 추가
- 새 기능 설명

### 수정
- 버그 수정 설명
```

#### 1-2. 버전 태그 생성 및 푸시

```bash
git add CHANGELOG.md
git commit -m "릴리즈 준비: v0.2.0"
git tag v0.2.0
git push origin main --tags
```

> **태그 형식:** `v{major}.{minor}.{patch}` (예: `v1.2.3`)  
> `-` 포함 태그(예: `v0.2.0-beta`)는 GitHub에서 **Pre-release**로 자동 표시됩니다.

#### 1-3. CI 진행 확인

GitHub → **Actions** 탭 → `Build and Release` 워크플로에서 각 단계를 확인합니다.

| 단계 | 내용 |
|---|---|
| Checkout source | 소스 체크아웃 |
| Setup .NET | .NET 8 SDK 설치 |
| Extract version | 태그에서 버전 추출 (`v0.2.0` → `0.2.0`) |
| Patch version in csproj | `NoxyTools.Wpf.csproj`의 Version/AssemblyVersion 자동 패치 |
| Restore NuGet packages | `dotnet restore` |
| Publish NoxyTools.Wpf | Self-contained Single-file (win-x64) publish |
| Install Inno Setup | Chocolatey로 Inno Setup 6 설치 |
| Compile Inno Setup script | `NoxyToolsSetup.exe` 인스톨러 생성 |
| Create GitHub Release | Release 생성 + `NoxyToolsSetup.exe` 첨부 |

전체 소요 시간: 약 5~10분

#### 1-4. 릴리즈 확인

GitHub → **Releases** 탭에서 새 릴리즈와 `NoxyToolsSetup.exe` 첨부 파일을 확인합니다.

---

## 2. 로컬 수동 빌드

### 사전 요구사항

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Inno Setup 6](https://jrsoftware.org/isdl.php) (기본 경로 설치: `C:\Program Files (x86)\Inno Setup 6\`)

### 빌드 절차

#### 2-1. noxypedia.dat 준비

`NoxypediaEditor`에서 최신 데이터를 내보낸 뒤 아래 경로에 복사합니다.

```
src/NoxyTools/NoxyTools.Wpf/Resources/noxypedia.dat
```

#### 2-2. Publish

```powershell
cd src/NoxyTools

dotnet publish NoxyTools.Wpf/NoxyTools.Wpf.csproj `
  -c Release `
  -r win-x64 `
  --self-contained true `
  -p:PublishSingleFile=true `
  -p:IncludeNativeLibrariesForSelfExtract=true `
  -o NoxyTools.Wpf/bin/Release/net8.0-windows/win-x64/publish
```

완료 후 출력 폴더를 확인합니다.

```
NoxyTools.Wpf/bin/Release/net8.0-windows/win-x64/publish/
├── NoxyToolsWpf.exe   ← 단일 실행 파일
└── noxypedia.dat
```

#### 2-3. 인스톨러 생성

```powershell
& "C:\Program Files (x86)\Inno Setup 6\ISCC.exe" "src/Setup/MakeSetup.iss"
```

또는 Inno Setup IDE에서 `src/Setup/MakeSetup.iss`를 열고 **Build → Compile**을 실행합니다.

인스톨러 생성 위치:

```
src/Setup/output/NoxyToolsSetup.exe
```

---

## 3. 버전 관리 규칙

[Semantic Versioning](https://semver.org/lang/ko/) 을 따릅니다.

| 변경 종류 | 올리는 자리 | 예시 |
|---|---|---|
| 하위 호환 버그 수정 | patch | `0.1.0` → `0.1.1` |
| 하위 호환 기능 추가 | minor | `0.1.0` → `0.2.0` |
| 하위 비호환 변경 | major | `0.1.0` → `1.0.0` |

버전은 [`src/NoxyTools/NoxyTools.Wpf/NoxyTools.Wpf.csproj`](../src/NoxyTools/NoxyTools.Wpf/NoxyTools.Wpf.csproj)의 `<Version>` 태그에 정의되어 있습니다.  
- **CI 배포 시**: 태그에서 자동 패치되므로 수동 수정 불필요  
- **로컬 수동 배포 시**: csproj를 직접 수정 후 빌드

---

## 4. 트러블슈팅

| 증상 | 원인 | 해결 |
|---|---|---|
| CI에서 `dotnet restore` 실패 | 솔루션 경로 오류 | `src/NoxyTools/NoxyTools.sln` 경로 확인 |
| Inno Setup 컴파일 실패 | `NoxyToolsWpf.exe`가 없음 | Publish 단계 로그 확인 |
| Release에 파일 없음 | `files:` 경로 불일치 | `src/Setup/output/NoxyToolsSetup.exe` 존재 여부 확인 |
| 앱 실행 시 데이터 없음 | `noxypedia.dat` 누락 | publish 폴더에 파일 포함 여부 확인 |
