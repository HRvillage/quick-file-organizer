<p align="center">
  <img src="assets/branding/banner.svg" alt="Quick File Organizer" width="100%">
</p>

<p align="center">
  <strong>適用於 Windows 的輕量、免安裝批次檔案重新命名工具。</strong><br>
  支援自訂名稱、日期來源、流水號、檔案類型分類、即時預覽與單次復原。
</p>

<p align="center">
  <a href="README.md">English</a> ·
  <a href="docs/zh-TW/UserGuide.md">使用教學</a> ·
  <a href="CHANGELOG.md">版本紀錄</a> ·
  <a href="docs/zh-TW/Development.md">開發歷程</a>
</p>

## 工具介紹

Quick File Organizer（檔案整理工具）用來快速替同一資料夾內的多個檔案套用一致的命名規則。操作介面維持單頁、固定尺寸，不需要複雜設定，也不會因為檔案數量增加而讓畫面變得擁擠。

發布版本採用可攜式封裝。完成編譯後，使用者只要解壓縮並執行 EXE，不需要另外安裝 Visual Studio、VS Code、.NET SDK 或其他開發工具。

## 主要功能

- 選擇資料夾後批次重新命名檔案
- 可組合「名稱 1、名稱 2、日期、流水號」
- 日期來源可選今日、手動輸入、建立時間或修改時間
- 依檔案時間排序後再編流水號
- 可延續相同命名組合上次使用的下一個號碼
- 檔案類型依圖片、影片、文件、壓縮檔分類
- 影片支援 MP4、MOV、AVI
- 執行前顯示即時單行預覽
- 可復原上一次重新命名操作
- 支援拖曳資料夾到視窗
- 可快速切換中文與英文介面
- 設定與復原紀錄保存在程式旁的 `Data` 資料夾
- 固定尺寸介面，不需要捲動，也不能手動拉伸

## 操作畫面

<p align="center">
  <img src="assets/screenshots/main-window-zh.png" alt="Quick File Organizer 中文介面" width="820">
</p>

## 下載與使用

請到 GitHub 專案的 **Releases** 頁面下載最新的可攜版壓縮檔。解壓縮後直接執行：

```text
QuickFileOrganizer.exe
```

不需要安裝。

## 快速使用

1. 選擇資料夾，或把一個資料夾拖曳到程式視窗。
2. 視需求輸入「名稱 1」與「名稱 2」，空白欄位會自動略過。
3. 設定日期來源、日期位置、起始號碼與流水號位數。
4. 勾選要處理的副檔名；也可以直接勾選整個檔案分類。
5. 確認即時預覽後，按下「開始重新命名」。
6. 若操作有誤，可使用「還原上一次」。

完整說明請參閱[繁體中文使用教學](docs/zh-TW/UserGuide.md)。

## 編譯方式

只有自行編譯原始碼時才需要：

- Windows 10 或 Windows 11
- .NET 8 SDK

執行：

```text
BUILD_RELEASE.bat
```

完成品會產生在：

```text
release\
```

一般使用者不需要安裝 .NET SDK、Visual Studio 或 VS Code。

## 文件

- [使用教學 — 繁體中文](docs/zh-TW/UserGuide.md)
- [User Guide — English](docs/en/UserGuide.md)
- [開發歷程 — 繁體中文](docs/zh-TW/Development.md)
- [Development Journey — English](docs/en/Development.md)
- [常見問題 — 繁體中文](docs/zh-TW/FAQ.md)
- [FAQ — English](docs/en/FAQ.md)
- [版本紀錄](CHANGELOG.md)
- [開發方向](ROADMAP.md)

## 隱私與安全

本工具完全在本機運作，不會上傳檔案、不會傳送檔名，也不需要登入帳號。偏好設定與上一次重新命名紀錄只會保存在程式旁的 `Data` 資料夾。

執行重新命名前，程式會先檢查檔名衝突，且不會覆蓋已存在的檔案。

## 授權

本專案採用 [MIT License](LICENSE)。

## 作者

© 2026 [HeroRaye](https://www.youtube.com/@HeroRaye)
