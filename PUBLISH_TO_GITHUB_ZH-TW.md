# GitHub 發布流程

Repository：

```text
https://github.com/HRvillage/quick-file-organizer
```

## 發布前檢查

1. 確認 `dotnet build src/QuickFileOrganizer/QuickFileOrganizer.csproj` 成功。
2. 確認程式可正常啟動。
3. 確認右下角版本顯示 `v1.1.0`。
4. 確認 EXE 版本為 `1.1.0`。
5. 確認 README、CHANGELOG、Release Notes 已更新。
6. 確認 Repository 不包含 `bin/`、`obj/`、`release/`、`.vs/` 或 `Data/settings.json`。
7. 確認 `git diff --check` 沒有 trailing whitespace 或 conflict marker。

## 建置 Portable 版本

在專案根目錄執行：

```text
BUILD_RELEASE.bat
```

建置完成後，`release` 資料夾應包含：

```text
QuickFileOrganizer.exe
README.txt
Data\
```

ZIP 檔名建議：

```text
QuickFileOrganizer_v1.1.0_Portable.zip
```

不要把 `release` 資料夾或 ZIP 建置產物提交到 Repository。

## 建立 GitHub Release

1. 前往 GitHub Repository 的 **Releases**。
2. 點選 **Draft a new release**。
3. 選擇 **Choose a tag**，輸入：

```text
v1.1.0
```

4. 選擇 **Create new tag: v1.1.0 on publish**。
5. Release title：

```text
Quick File Organizer v1.1.0
```

6. 將 `RELEASE_NOTES_v1.1.0.md` 的內容貼到說明欄。
7. 上傳：

```text
QuickFileOrganizer_v1.1.0_Portable.zip
```

8. 不勾選 Pre-release。
9. 點選 **Publish release**。

## 版本規則

- 修補問題：`v1.1.1`
- 新增相容功能：`v1.2.0`
- 重大不相容變更：`v2.0.0`

版本號需要同步更新：

- `src/QuickFileOrganizer/QuickFileOrganizer.csproj`
- `src/QuickFileOrganizer/app.manifest`
- MainForm 右下角版本文字
- `PORTABLE_README.txt`
- `CHANGELOG.md`
- Release Notes
- Git tag 與 GitHub Release title
