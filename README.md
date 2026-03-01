# NoxyTools

Warcraft III 커스텀 맵 **Noxirian** 플레이를 보조하는 Windows 데스크톱 도구입니다.

## 주요 기능

| 탭 | 설명 |
|---|---|
| **검증 보고서** | 세이브 파일을 분석하여 카페 업로드용 아이템 검증 보고서를 자동 생성합니다. 최대 6개 슬롯을 지원합니다. |
| **아이템 검색** | Noxypedia 데이터베이스를 기반으로 아이템·크리처·제작 레시피 등의 정보를 검색합니다. |
| **아이템 시뮬레이터** | 아이템 조합의 스탯 시뮬레이션 및 프리셋 관리를 지원합니다. |

추가 기능:
- **설정 백업/복구** — 모든 사용자 설정을 `.noxconfig` 파일로 내보내거나 불러올 수 있습니다.
- **자동 업데이트 확인** — 시작 시 GitHub Releases를 통해 최신 버전을 확인합니다.

## 빌드 요구사항

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Visual Studio 2022 이상 (WPF 워크로드 포함) 또는 `dotnet` CLI

## 빌드 방법

```bash
# 솔루션 루트에서
dotnet build src/NoxyTools/NoxyTools.sln
```

또는 Visual Studio에서 `src/NoxyTools/NoxyTools.sln`을 열고 빌드합니다.

### 배포 빌드

```bash
dotnet publish src/NoxyTools/NoxyTools.Wpf/NoxyTools.Wpf.csproj \
  -c Release -r win-x64 --self-contained false
```

인스톨러는 `src/Setup/MakeSetup.iss` (Inno Setup 6 이상)를 사용합니다.

## 데이터 파일

`noxypedia.dat` 파일은 빌드 출력 디렉터리(`bin/`)에 위치해야 합니다.  
파일은 **NoxypediaEditor** 프로젝트로 생성·수출합니다.

## 설정 저장 위치

사용자 설정은 Windows 레지스트리 `HKCU\Software\NoxyTools\Config` 에 저장됩니다.  
상단 메뉴의 **⬆ 백업** / **⬇ 복구** 버튼으로 파일 단위 백업 및 복원이 가능합니다.

## 프로젝트 구조

```
src/NoxyTools/
├── NoxyTools.Wpf/          # WPF UI 진입점 (MVVM)
├── NoxyTools.Core/         # 비즈니스 로직 및 서비스
├── Noxypedia/              # 아이템 데이터베이스 라이브러리
├── NoxypediaEditor/        # Noxypedia 데이터 편집 도구 (WinForms)
├── ReplayParser/           # 리플레이 파일 파서
├── SaveParser/             # 세이브 파일 파서
└── Settings/               # 레지스트리 래퍼
```

## 라이선스

이 프로젝트는 개인 사용 목적으로 작성되었습니다.
