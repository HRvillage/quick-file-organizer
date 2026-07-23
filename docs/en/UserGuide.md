# Quick File Organizer User Guide

## 1. Launch the Application

Run the release executable:

```text
QuickFileOrganizer.exe
```

The adjacent `Data` folder stores preferences and the most recent rename record. Do not delete it if you still need to use Undo.

## 2. Select a Folder

Use either method:

- Select **Choose Folder**.
- Drag one folder into the application window.

Only files in the selected folder's top level are processed. Subfolders and folder names are not renamed.

## 3. Configure Standard Naming

### Names and Dates

`Name 1` and `Name 2` are optional. Empty fields are skipped automatically without creating extra separators.

Date source options:

- No date
- Today
- Manual
- File date

File date can use either modified time or created time. If files were downloaded, copied, or exported, review the Windows file properties before choosing the time source.

### Filename Format

The filename format dropdown provides common patterns, such as:

```text
Name1_Name2_Date_Number
Name1_Name2_Number_Date
Date_Name1_Name2_Number
Number_Name1_Name2_Date
```

It also includes a name-plus-number format without an underscore between the name and number, which can be useful for products, cases, and repeated field records.

### Sequence Number

- **Start number** sets the first number for the current operation.
- **Digits** controls zero padding, such as `001`.
- **Continue matching name combination** resumes from the next stored number for the same naming combination.

## 4. Select File Types

Available categories:

- Images: JPG/JPEG, PNG, GIF, WEBP, BMP
- Videos: MP4, MOV, AVI
- Documents: PDF, Word, Excel, PowerPoint, TXT
- Archives: ZIP, RAR, 7Z
- Audio: MP3, WAV, OGG, WMA, AAC
- Advanced images: HEIC, TIFF, RAW
- Design assets: PSD, AI, INDD, Sketch, FIG
- CAD drawings: DWG, DWF, DXF
- 3D models: STEP, IGES, STL, 3DS

Selecting a category selects all formats in that category. Individual formats can still be selected separately. File type selections reset to off when the application restarts to reduce accidental operations.

## 5. Use Any Mode

If the extension you need is not in the preset list, enable **Any: include all files in the folder**.

Any mode includes all non-hidden, non-system files in the selected folder and ignores the preset file type list. The application shows a confirmation dialog before enabling it. Review the folder contents and preview carefully before renaming.

## 6. Use the Grouped Naming Assistant

Select **Grouped naming...** to open the Grouped Naming Assistant.

Common uses include:

- Construction records: before, during, after
- Insurance claims: front, rear, left, right
- Real estate: living room, dining room, kitchen
- E-commerce products: front, back, side
- Medical records: before, during, after procedure
- Airbnb rooms: living room, bedroom, bathroom

Basic steps:

1. Set the number of files per group.
2. Choose the sort order.
3. Set the group prefix, start number, digits, and separator.
4. Choose same name or different names inside each group.
5. Add a date marker if needed and choose its position.
6. Review grouped preview and time gap warnings.
7. Rename after confirming the preview.

Time gap warnings help identify possible missing files, extra files, unexpected files, or sort order issues. They do not automatically fix grouping. Check the folder, remove or add files if needed, and preview again.

## 7. Review and Rename

Before renaming, confirm:

- The selected folder is correct.
- The filename format is correct.
- Date source and sequence number are correct.
- File type selection or Any mode matches the intended batch.
- The preview does not include unexpected files.

If the batch contains multiple extensions, the application asks for confirmation before continuing.

## 8. Undo the Last Operation

Select **Undo Last** to revert the most recent successful rename operation.

Notes:

- Only the latest successful operation can be undone.
- Files that were moved, deleted, or renamed again may not be recoverable.
- A new successful rename replaces the previous undo record.

## 9. Change Language

Use the language switch in the upper-right corner to change between Chinese and English. The Grouped Naming Assistant follows the language selected in the main window.
