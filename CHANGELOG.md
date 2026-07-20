# Changelog

All notable public changes to this project will be documented in this file.

## [1.0.1] - 2026-07-20

### Fixed

- Improved DPI scaling and layout behavior across different Windows display settings.
- Fixed controls being clipped or compressed on some laptop displays.
- Improved the height and spacing of the File Types section.
- Improved button sizing for Chinese and English interface text.

### Changed

- The main window can now be resized and maximized.
- Added vertical scrolling as a fallback on limited screen space.
- Added a custom Windows application icon.
- Updated project metadata and repository information.

## [1.0.0] - 2026-07-17

### Added

- Batch file renaming from a selected folder.
- Optional Name 1 and Name 2 segments.
- Date sources: no date, today, manual date, created time, and modified time.
- Date position before or after custom names.
- Configurable starting number and sequence digit count.
- Continued numbering for matching naming combinations.
- File type categories for images, videos, documents, and archives.
- Category-level select all with individual extension control.
- Video support for MP4, MOV, and AVI.
- Live single-line filename preview.
- Folder drag-and-drop.
- Conflict detection before renaming.
- Temporary two-phase rename flow to avoid internal filename collisions.
- One-step undo for the latest successful rename.
- Chinese and English interface.
- Portable local settings and undo data.
- Clickable HeroRaye copyright link.

### Interface

- Compact card-based Windows interface.
- Fixed-size single-screen layout without scrolling.
- Clear primary, secondary, and undo action styles.
- Optimized language switching to update interface text in one refresh.

### Distribution

- .NET 8 self-contained single-file Windows build.
- Portable `Data` directory for preferences and last rename record.
- No executable compression in the release build.
