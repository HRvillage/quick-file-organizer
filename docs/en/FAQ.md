# Frequently Asked Questions

## Do I need .NET or development tools to run the application?

No. The release executable on GitHub Releases includes the required runtime. End users only need to download and run `QuickFileOrganizer.exe`.

The .NET 8 SDK is required only when building from source.

## Does the application upload my files or filenames?

No. All processing is local. The application requires no account and sends no file or filename data to an online service.

## Why are all file types cleared after every restart?

This is an intentional safety choice that reduces the chance of running a bulk rename against an unintended group of files.

## What is Any mode?

Any mode is for extensions that are not included in the preset list. When enabled, the application includes all non-hidden, non-system files in the selected folder and asks for confirmation before continuing.

Review the folder contents and preview carefully when using Any mode because it does not filter by file type.

## Are subfolders processed?

No. Only files in the top level of the selected folder are processed.

## Can it rename folders?

No. The current version renames files only.

## What is the difference between created and modified time?

Created time often represents when a file entered the current disk or folder and may change after copying, downloading, or extraction. Modified time usually represents the last content change and may preserve a more meaningful date for some photos or downloaded files.

Review the Windows file properties before choosing the time source for important batches.

## Is the Grouped Naming Assistant only for photos?

No. It can be used with any files included by the current file type selection, or with Any mode for other extensions. It is useful whenever files repeat in fixed-size groups.

## Do time gap warnings automatically fix grouping issues?

No. Time gap warnings only point out where a group may contain missing files, extra files, unrelated files, or unexpected ordering. Check the folder, adjust the files, and preview again.

## Can Undo restore multiple operations?

No. The application stores only the latest successful rename operation.

## Can a moved file still be restored?

A file that was moved, deleted, or renamed again may not be recoverable. The application attempts to restore entries that are still available and reports the result.

## Why might Windows or antivirus software display a warning?

Unsigned, independently distributed Windows applications can occasionally trigger SmartScreen or antivirus reputation checks. Download only from this repository's official Releases page and verify the release source and version.

## Can I run it from a USB drive or network location?

Yes, provided the application can write to the adjacent `Data` folder. Preferences and undo history cannot be saved in a read-only location.
