<p align="center">
  <img src="assets/branding/banner.svg" alt="Quick File Organizer" width="100%">
</p>

<p align="center">
  <strong>輕量、可攜式的 Windows 批次檔案重新命名工具。</strong><br>
  支援自訂名稱、日期來源、流水號、檔案類型分類、即時預覽與一鍵還原。
</p>

<p align="center">
  <a href="README.md">English</a> |
  <a href="docs/zh-TW/UserGuide.md">使用指南</a> |
  <a href="CHANGELOG.md">更新紀錄</a> |
  <a href="docs/zh-TW/Development.md">開發紀錄</a>
</p>

## 專案概覽

Quick File Organizer 可協助你用一致的命名格式批次整理檔案，同時保留清楚、易懂的操作流程。它是可攜式 Windows 工具：下載自包含套件、解壓縮後執行 EXE，不需要另外安裝 .NET Runtime。

應用程式提供可調整大小的 Windows 介面、最大化支援、DPI 顯示縮放相容性、有限螢幕空間下的垂直捲動備援，以及正式的 Windows 應用程式圖示。

## 主要功能

- 從指定資料夾批次重新命名檔案
- 可組合名稱 1、名稱 2、日期與流水號
- 日期來源支援今天、手動日期、建立時間與修改時間
- 可依檔案時間排序後再編號
- 可依相同命名組合接續上一次編號
- 依圖片、影片、文件與壓縮檔分類選擇檔案類型
- 影片格式支援 MP4、MOV 與 AVI
- 重新命名前提供單行即時預覽
- 可還原最近一次成功的重新命名操作
- 支援資料夾拖放
- 支援中文與英文介面切換
- 設定與操作資料儲存在本機 `Data` 資料夾
- 可調整大小並支援最大化的 Windows 介面
- 支援不同 Windows DPI 顯示縮放設定
- 螢幕空間有限時提供垂直捲動備援
- 內建自訂 Windows 應用程式圖示

## 畫面截圖

<p align="center">
  <img src="assets/screenshots/main-window-zh.png" alt="Quick File Organizer 主畫面" width="820">
</p>

## 下載

請從 GitHub Repository 的 **Releases** 頁面下載可攜式 ZIP，解壓縮後執行：

```text
QuickFileOrganizer.exe
```

不需要安裝，也不需要另外安裝 .NET Runtime。

## 快速開始

1. 選擇資料夾，或將資料夾拖放到視窗中。
2. 視需要輸入名稱 1 與名稱 2。
3. 選擇日期來源、日期位置、起始號碼與流水號位數。
4. 選擇一個或多個檔案類型，或直接選擇整個分類。
5. 確認預覽結果後，按下 **開始重新命名**。
6. 若最近一次操作需要復原，可使用 **還原上一次**。

完整說明請參考 [繁體中文使用指南](docs/zh-TW/UserGuide.md) 或 [English User Guide](docs/en/UserGuide.md)。

## 建置

僅在自行建置時需要：

- Windows 10 或 Windows 11
- .NET 8 SDK

執行：

```text
BUILD_RELEASE.bat
```

自包含的可攜式版本會輸出到：

```text
release\
```

一般使用者不需要 .NET SDK、Visual Studio 或 VS Code。

## 文件

- [使用指南 - 繁體中文](docs/zh-TW/UserGuide.md)
- [User Guide - English](docs/en/UserGuide.md)
- [開發紀錄 - 繁體中文](docs/zh-TW/Development.md)
- [Development Journey - English](docs/en/Development.md)
- [常見問題 - 繁體中文](docs/zh-TW/FAQ.md)
- [FAQ - English](docs/en/FAQ.md)
- [更新紀錄](CHANGELOG.md)
- [Roadmap](ROADMAP.md)

## 意見回饋

如果你發現問題或有功能建議，歡迎到 GitHub Issues 回報：

https://github.com/HRvillage/quick-file-organizer/issues

## 隱私與安全

Quick File Organizer 在本機運作，不會上傳檔案、不會傳送檔名，也不需要線上帳號。重新命名紀錄與偏好設定會儲存在應用程式旁的 `Data` 資料夾。

重新命名前，程式會檢查檔名衝突，不會覆蓋既有檔案。

## 授權

本專案採用 [MIT License](LICENSE) 授權。

## 作者

Copyright 2026 [HeroRaye](https://www.youtube.com/@HeroRaye)
