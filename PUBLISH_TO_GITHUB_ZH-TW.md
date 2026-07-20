# GitHub 發布操作指引

本指引假設 Repository 名稱使用：

```text
quick-file-organizer
```

## 第一階段：建立 Repository

1. 登入 GitHub。
2. 右上角按 `+`，選擇 **New repository**。
3. 填入：

```text
Repository name: quick-file-organizer
Description: A lightweight Windows batch file renaming tool with customizable naming rules.
```

4. 建議選擇 **Public**。
5. 不要勾選「Add a README file」、「Add .gitignore」或「Choose a license」，因為專案資料夾內已經準備完成。
6. 按下 **Create repository**。

## 第二階段：在本機建立 Git 紀錄

在專案根目錄開啟 Terminal，依序執行：

```powershell
git init
git add .
git commit -m "Release Quick File Organizer v1.0.1"
git branch -M main
```

把下方網址中的 GitHub 帳號改成你的實際帳號：

```powershell
git remote add origin https://github.com/HRvillage/quick-file-organizer.git
git push -u origin main
```

若 Repository 位置不同，也要同步修改：

- `src/QuickFileOrganizer/QuickFileOrganizer.csproj` 的 `RepositoryUrl`
- README 中的 GitHub 專案網址

YouTube 版權連結不需要修改。

## 第三階段：設定 Repository 首頁

進入 GitHub Repository 後，在右側 **About** 區塊按齒輪，建議設定：

```text
Description:
A lightweight Windows batch file renaming tool with customizable naming rules.
```

Topics：

```text
windows
winforms
csharp
dotnet
batch-rename
file-renamer
file-organizer
portable-app
desktop-utility
```

Website 可以先放：

```text
https://www.youtube.com/@HeroRaye
```

之後也可改成 GitHub Pages 或專案介紹頁。

## 第四階段：編譯正式版

在專案根目錄執行：

```text
BUILD_RELEASE.bat
```

確認 `release` 資料夾內至少包含：

```text
QuickFileOrganizer.exe
README.txt
Data\
```

請實際測試：

- 程式能正常啟動。
- 右下角版本顯示 `v1.0.1`。
- 中文與英文切換正常。
- 重新命名與還原功能正常。
- 換到一台沒有 .NET SDK 的 Windows 電腦仍能執行。

## 第五階段：製作 Portable ZIP

將以下內容壓縮：

```text
QuickFileOrganizer.exe
README.txt
Data\
```

ZIP 檔名建議：

```text
QuickFileOrganizer_v1.0.1_Portable.zip
```

不要把整個原始碼或 `release` 外層資料夾包進 Portable ZIP。

## 第六階段：建立 GitHub Release

1. Repository 首頁右側點 **Releases**。
2. 點 **Draft a new release**。
3. 選擇 **Choose a tag**，輸入：

```text
v1.0.1
```

4. 選擇 **Create new tag: v1.0.1 on publish**。
5. Release title：

```text
Quick File Organizer v1.0.1
```

6. 將 `RELEASE_NOTES_v1.0.1.md` 的內容貼到說明欄。
7. 上傳：

```text
QuickFileOrganizer_v1.0.1_Portable.zip
```

8. 不要勾選 Pre-release。
9. 按下 **Publish release**。

## 第七階段：完成後檢查

- README 圖片與連結是否正常。
- 中文 README 是否能從首頁點入。
- Release ZIP 是否能下載並正常解壓縮。
- EXE 版本是否為 `1.0.1`。
- Repository 內沒有 `bin`、`obj`、`release` 或個人設定檔。
- `Data/settings.json` 沒有被上傳。

## 後續更新版本規則

- 小錯誤修正：`v1.0.1`
- 新增相容功能：`v1.1.0`
- 大幅改變操作或相容性：`v2.0.0`

每次發布前同步更新：

- `QuickFileOrganizer.csproj`
- 程式右下角版本文字
- `CHANGELOG.md`
- Release Notes
- Git tag 與 Release title
