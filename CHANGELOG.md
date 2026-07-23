# Changelog

All notable public changes to this project will be documented in this file.

## [1.1.0] - 2026-07-23

### Added

- Added a grouped naming assistant for repeated file sets.
- Added grouped naming preview with time gap warnings.
- Added common filename format presets, including formats where the sequence number is attached directly to the name.
- Added multi-extension confirmation before renaming mixed file batches.
- Added support for audio formats: MP3, WAV, OGG, WMA, and AAC.
- Added support for advanced image formats: HEIC, TIFF, and common RAW formats.
- Added support for design, CAD, and 3D model formats including PSD, AI, INDD, Sketch, FIG, DWG, DWF, DXF, STEP, IGES, STL, and 3DS.
- Added Any mode for renaming all non-hidden, non-system files when the needed extension is not in the preset list.

### Changed

- Reframed grouped naming as a general file workflow instead of a photo-only workflow.
- Updated Chinese and English UI text for clearer grouped naming and time gap guidance.
- Updated project version metadata to v1.1.0.

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
