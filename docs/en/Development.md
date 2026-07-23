# Development Journey

## Project Origin

Quick File Organizer began with a common but repetitive task: renaming large sets of photos, documents, design assets, or other work files according to consistent rules. Windows File Explorer works well for small edits, but manual work becomes error-prone when filenames require dates, custom name segments, sequence numbers, file timestamps, or grouped patterns.

The project is therefore built around four goals: speed, clarity, safety, and portability. It is intentionally a focused batch naming utility rather than a large file management suite.

## Design Principles

### Keep the workflow low-friction

The primary workflow stays in one window so users can choose a folder, configure naming rules, select file types, review the preview, and rename files without moving through complex screens.

### Make naming rules understandable

The standard workflow is based on practical filename elements: names, dates, and sequence numbers. Common filename format presets are provided so users do not need to learn a complex template language.

### Reduce bulk operation risk

The application provides previews, target-name conflict checks, mixed-extension confirmation, Any mode confirmation, temporary two-phase renaming, and one-step undo.

### Stay local and portable

The application is distributed as a portable Windows tool. Settings and undo records are stored beside the executable in the `Data` folder. All file processing remains local and requires no account or online service.

## Feature Evolution

### Initial prototype

The first stage established folder selection, basic custom naming, date handling, sequence numbering, and batch renaming sorted by file time.

### v1.0.0

The first public release was normalized as `v1.0.0`. It established the core rename workflow, preview, undo, file type filtering, portable publishing, and GitHub project documentation.

### v1.0.1

This maintenance release focused on Windows desktop compatibility. It improved DPI behavior, laptop layout handling, resizable and maximizable window support, AutoScroll fallback, button text sizing, File Types height, rounded button rendering, and the official Windows app icon.

### v1.1.0

This release added the Grouped Naming Assistant for repeated file sets, such as construction records, insurance claim cases, real estate rooms, product photos, medical records, Airbnb rooms, and other grouped workflows. It supports fixed group sizes, same or different names inside each group, grouped previews, time gap warnings, and grouped date markers.

v1.1.0 also expanded file type support with audio, advanced image, design asset, CAD drawing, and 3D model categories. It introduced Any mode for confirmed renaming of extensions that are not included in the preset list.

## Technology

- C#
- .NET 8
- Windows Forms
- Self-contained single-file publishing
- Local JSON settings and undo records

WinForms was selected for fast startup, direct Windows integration, low dependency overhead, and suitability for a lightweight focused desktop utility.

## Future Direction

Future releases will prioritize compatibility, naming flexibility, and safety improvements discovered through real use. New features will be considered when they preserve the application's low-friction workflow and avoid unnecessary complexity.
