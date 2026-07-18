<p align="center">
  <img src="assets/branding/banner.svg" alt="Quick File Organizer" width="100%">
</p>

<p align="center">
  <strong>A lightweight, portable batch file renaming utility for Windows.</strong><br>
  Custom names, date sources, sequence numbering, grouped file types, preview, and one-step undo.
</p>

<p align="center">
  <a href="README.zh-TW.md">繁體中文</a> ·
  <a href="docs/en/UserGuide.md">User Guide</a> ·
  <a href="CHANGELOG.md">Changelog</a> ·
  <a href="docs/en/Development.md">Development Journey</a>
</p>

## Overview

Quick File Organizer helps rename groups of files with a consistent naming pattern while keeping the workflow compact and easy to understand. It is designed as a portable Windows utility: build the self-contained release once, then run the executable without installing development tools or a separate .NET runtime.

## Features

- Batch rename files from a selected folder
- Combine Name 1, Name 2, date, and sequence number
- Date options: today, manual date, created time, or modified time
- Sort files by file time before numbering
- Continue numbering from the previous matching name combination
- Grouped file type selection for images, videos, documents, and archives
- Supported video formats include MP4, MOV, and AVI
- Live single-line preview before renaming
- One-step undo for the most recent rename operation
- Folder drag-and-drop
- Chinese and English interface switch
- Portable data storage in the local `Data` folder
- Fixed-size compact interface with no scrolling

## Screenshot

<p align="center">
  <img src="assets/screenshots/main-window-zh.png" alt="Quick File Organizer main window" width="820">
</p>

> **Known Issue:** Minor UI scaling issues may occur on some laptops using 125% or 150% Windows display scaling. This will be addressed in **v1.0.1**.

**Planned Fix:** Monday (v1.0.1)

The upcoming update will include:

- Improved DPI scaling support
- Better layout compatibility across different screen resolutions
- Optimized control spacing and sizing

Thank you for your patience and support.

## Download

Download the latest portable package from the repository's **Releases** page. Extract the ZIP and run:

```text
QuickFileOrganizer.exe
```

No installation is required.

## Quick Start

1. Select or drag a folder into the window.
2. Enter optional values for Name 1 and Name 2.
3. Choose the date source, date position, starting number, and number of digits.
4. Select one or more file types, or select an entire category.
5. Confirm the preview and choose **Start Rename**.
6. Use **Undo Last** if the most recent operation needs to be reverted.

See the full [English user guide](docs/en/UserGuide.md) or [Traditional Chinese user guide](docs/zh-TW/UserGuide.md).

## Build

Requirements for building only:

- Windows 10 or Windows 11
- .NET 8 SDK

Run:

```text
BUILD_RELEASE.bat
```

The self-contained portable build will be created in:

```text
release\
```

End users do not need the .NET SDK, Visual Studio, or VS Code.

## Documentation

- [User Guide — English](docs/en/UserGuide.md)
- [使用教學 — 繁體中文](docs/zh-TW/UserGuide.md)
- [Development Journey — English](docs/en/Development.md)
- [開發歷程 — 繁體中文](docs/zh-TW/Development.md)
- [FAQ — English](docs/en/FAQ.md)
- [常見問題 — 繁體中文](docs/zh-TW/FAQ.md)
- [Changelog / 版本紀錄](CHANGELOG.md)
- [Roadmap / 開發方向](ROADMAP.md)

## Privacy and Safety

Quick File Organizer works locally. It does not upload files, transmit filenames, or require an online account. Rename history and preferences are stored beside the application in the `Data` folder.

Before renaming, the application checks for filename conflicts and does not overwrite existing files.

## License

This project is licensed under the [MIT License](LICENSE).

## Author

© 2026 [HeroRaye](https://www.youtube.com/@HeroRaye)
