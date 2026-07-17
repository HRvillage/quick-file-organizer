# Quick File Organizer User Guide

## 1. Launch the Application

Extract the portable ZIP and run:

```text
QuickFileOrganizer.exe
```

The local `Data` folder stores preferences and the most recent rename record. Do not delete it before using Undo.

## 2. Select a Folder

Use either method:

- Select **Choose Folder**.
- Drag one folder into the application window.

Only files in the selected folder's top level are processed. Subfolders and folder names are not changed.

## 3. Configure the New Filename

### Name 1 and Name 2

Both fields are optional. Empty fields are skipped automatically without producing duplicate separators.

Example:

```text
Name 1: Bandai
Name 2: OPCG
```

### Date Source

- **No date**: do not include a date.
- **Today**: use the current date.
- **Manual**: choose a specific date.
- **File time**: use each file's created or modified timestamp.

For files copied, downloaded, or exported from another device, the meaningful original time may be stored as the modified time. Check the Windows file properties before processing important files.

### Date Position

- **Before names**: `20260717_Bandai_OPCG_001.jpg`
- **After names**: `Bandai_OPCG_20260717_001.jpg`

### Sequence Number

- **Start number** sets the first number used in the current operation.
- **Digits** controls zero padding, such as `001` for three digits.
- **Continue matching name combination** resumes from the next number previously stored for the same naming combination.

## 4. Select File Types

Available categories:

- Images: JPG/JPEG, PNG, GIF, WEBP, BMP
- Videos: MP4, MOV, AVI
- Documents: PDF, Word, Excel, PowerPoint, TXT
- Archives: ZIP, RAR, 7Z

Selecting a category selects all formats in that category. Clearing the category clears all of them. Individual formats can still be selected separately.

File type selections reset to off whenever the application is restarted to reduce accidental operations.

## 5. Review the Preview

The preview displays a filename example based on the current settings. After a valid folder and file types are selected, the application also reports matching files.

Before renaming, confirm:

- The selected folder is correct.
- Name and date order are correct.
- Start number and digit count are correct.
- The intended file types are selected.

## 6. Rename Files

Select **Start Rename**. The application checks for naming conflicts before applying changes.

Existing files are never overwritten. If a target filename already exists, the operation stops and reports the conflict.

Use **Open Folder** after completion to review the result.

## 7. Undo the Last Operation

Select **Undo Last** to revert the most recent successful rename operation.

Notes:

- Only the latest operation performed by this application can be undone.
- Files that were deleted, moved, or renamed again may not be recoverable.
- A new successful rename replaces the previous undo record.

## 8. Change Language

Use the compact language switch in the upper-right corner to change between Chinese and English. The selected language is saved.

## 9. Move the Portable Application

Copy both:

```text
QuickFileOrganizer.exe
Data\
```

Delete the `Data` folder only when saved preferences and undo history are no longer needed. The application recreates it when required.
