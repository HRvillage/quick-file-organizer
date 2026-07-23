<p align="center">
  <img src="assets/branding/banner.svg" alt="Quick File Organizer" width="100%">
</p>

<p align="center">
  <strong>輕量、可攜的 Windows 批次檔案重新命名工具。</strong><br>
  支援彈性檔名格式、分組命名、常見工作檔案類型、即時預覽與一鍵還原。
</p>

<p align="center">
  <a href="README.md">English</a> |
  <a href="docs/zh-TW/UserGuide.md">使用說明</a> |
  <a href="CHANGELOG.md">更新紀錄</a> |
  <a href="docs/zh-TW/Development.md">開發紀錄</a>
</p>

## 簡介

Quick File Organizer 可以協助你把日常工作檔案整理成一致、清楚、可追蹤的命名格式。它適合施工照片、保險理賠、房仲物件、電商商品圖、設計素材、AI 生成圖片、掃描文件，以及其他需要重複整理的檔案集合。

這是一個可攜式 Windows 工具。下載自包含版本、解壓縮後直接執行，不需要另外安裝 .NET Runtime。

程式介面支援視窗調整大小、最大化、Windows DPI scaling、空間不足時的垂直捲動備援，以及正式 Windows app icon。

## 功能

- 從指定資料夾批次重新命名檔案
- 組合名稱 1、名稱 2、日期與流水號
- 提供常用檔名格式，包含「名稱接流水號」這類無底線格式
- 日期來源支援今天日期、手動日期、建立時間與修改時間
- 依檔案時間或檔名排序後重新命名
- 可接續相同命名設定的上一個流水號
- 分組命名助手，適合固定數量、重複項目的檔案集合
- 分組預覽可提醒時間間隔異常，協助檢查漏拍、混入無關檔案或排序錯誤
- 檔案類型支援圖片、進階圖片、影片、音訊、文件、壓縮檔、設計檔、CAD 圖面與 3D 模型
- 支援 JPG、PNG、HEIC、TIFF、RAW、MP4、MOV、MP3、WAV、PDF、Office、ZIP、PSD、AI、INDD、DWG、DXF、STEP、STL 等格式
- Any 模式可納入資料夾內所有非隱藏、非系統檔案，適合處理不在預設清單中的副檔名
- 同一批次包含多種副檔名時會先提醒確認
- 重新命名前提供即時預覽
- 可還原最近一次成功的重新命名
- 支援資料夾拖曳
- 支援中文與英文介面
- 設定與還原紀錄儲存在本機 `Data` 資料夾

## 畫面

<p align="center">
  <img src="assets/screenshots/main-window-zh.png" alt="Quick File Organizer 主視窗" width="820">
</p>

## 下載

請從 GitHub Repository 的 **Releases** 頁面下載 portable ZIP，解壓縮後執行：

```text
QuickFileOrganizer.exe
```

不需要安裝，也不需要另外安裝 .NET Runtime。

## 快速開始

1. 選擇資料夾，或把資料夾拖曳到視窗中。
2. 輸入需要的名稱 1 與名稱 2。
3. 選擇日期來源、檔名格式、起始號碼與流水號位數。
4. 勾選要處理的檔案類型、整個分類，或在副檔名不在清單中時使用 Any 模式。
5. 確認預覽後按下 **開始重新命名**。
6. 若需要復原，可使用 **還原上一次**。

詳細操作請參考 [繁體中文使用說明](docs/zh-TW/UserGuide.md) 或 [English User Guide](docs/en/UserGuide.md)。

## 建置

建置需求：

- Windows 10 或 Windows 11
- .NET 8 SDK

執行：

```text
BUILD_RELEASE.bat
```

自包含 portable build 會產生在：

```text
release\
```

一般使用者不需要 .NET SDK、Visual Studio 或 VS Code。

## 文件

- [使用說明 - 繁體中文](docs/zh-TW/UserGuide.md)
- [User Guide - English](docs/en/UserGuide.md)
- [開發紀錄 - 繁體中文](docs/zh-TW/Development.md)
- [Development Journey - English](docs/en/Development.md)
- [常見問題 - 繁體中文](docs/zh-TW/FAQ.md)
- [FAQ - English](docs/en/FAQ.md)
- [更新紀錄](CHANGELOG.md)
- [Roadmap](ROADMAP.md)

## 回饋

如果你發現問題或有功能建議，歡迎在 GitHub Issues 回報：

https://github.com/HRvillage/quick-file-organizer/issues

## 隱私與安全

Quick File Organizer 在本機執行，不會上傳檔案、不會傳送檔名，也不需要線上帳號。設定與重新命名紀錄會儲存在程式旁邊的 `Data` 資料夾。

重新命名前，程式會檢查同名衝突，不會覆蓋既有檔案。

## 授權

本專案採用 [MIT License](LICENSE)。

## 作者

Copyright 2026 [HeroRaye](https://www.youtube.com/@HeroRaye)
