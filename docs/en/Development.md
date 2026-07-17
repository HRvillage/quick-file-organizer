# Development Journey

## Project Origin

Quick File Organizer began with a common but repetitive task: renaming groups of images, videos, and documents according to a consistent rule. Windows File Explorer works well for small edits, but manual work becomes error-prone when filenames require dates, multiple custom name segments, sequential numbering, or time-based ordering.

The project was therefore shaped around four goals: speed, clarity, portability, and safe recovery. It is intentionally a focused utility rather than a large file management suite.

## Design Principles

### Complete the task on one screen

The primary workflow stays in a single window without nested menus or tabs. The public version uses a fixed compact layout so all controls and action buttons remain visible together.

### Prefer clarity over configuration depth

The filename is built from four practical elements: Name 1, Name 2, date, and sequence number. Empty elements are skipped automatically, avoiding the need for a separate template manager.

### Reduce accidental operations

File types start unselected on every launch. The interface provides a live preview, checks target-name conflicts before processing, and stores one undo record after a successful operation.

### Portable and local-first

The application uses a portable distribution and stores preferences beside the executable in the `Data` folder. All file processing remains local and requires no account or online service.

## Feature Evolution

### Initial prototype

The first stage established folder selection, basic custom naming, a date prefix, sequence numbering, and ordering by file creation time.

### Workflow expansion

The tool later gained a second custom name field, configurable date position, manual dates, created or modified file timestamps, continuation numbering, file type filtering, and direct folder opening after completion.

### Safety and accessibility

Preview, conflict detection, temporary two-phase renaming, and one-step undo were introduced to reduce the risk of bulk changes. File formats were grouped as images, videos, documents, and archives so users do not need detailed extension knowledge.

### Interface refinement

The interface moved from default WinForms presentation to a compact card-based layout with clearer button hierarchy, spacing, color, and Chinese/English switching. After testing across display sizes and scaling settings, the release layout was fixed and scrolling was removed to prevent hidden action buttons.

### v1.0.0

The first public release was normalized as `v1.0.0`. Before publication, the source was cleaned, project documentation was prepared in both languages, and the portable release and GitHub repository structure were standardized.

## Technology

- C#
- .NET 8
- Windows Forms
- Self-contained single-file publishing
- Local JSON settings and undo record

WinForms was selected for fast startup, direct Windows integration, low dependency overhead, and suitability for a focused desktop utility.

## Future Direction

Future releases will prioritize compatibility fixes and practical improvements discovered through real use. New features will be considered only when they preserve the application's compact, single-screen workflow.
