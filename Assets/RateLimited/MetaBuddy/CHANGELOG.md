# MetaBuddy Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [1.0.0] - 2020-09-04
### Added
- Check staged changes for meta file errors.
- Check entire project for meta file errors.
- Command line options.

## [1.1.8] - 2020-10-01
### Added
- Check for directories that have been added without a meta file.
- Check for directories that have been deleted without a meta file.
- Improve Unity Editor responsiveness when checking large projects.
- Release versioning.

## [1.2.17] - 2020-11-13
### Added
- "Highlight in project" in error context menu.
- "How to fix..." item in error context menu.
- Copy project relative path in error context menu.

### Changed
- Group copy to clipboard items into a sub-menu.

### Fixed 
- Show paths relative to Unity project root, rather than git repository root.
- Broken link to Discord server.

## [1.3.31] - 2020-11-21 
### Added
- Support & feedback links in window footer.
- Ignores assets that Unity regards as hidden (see https://docs.unity3d.com/Manual/SpecialFolders.html)
- Config option to log ignored files to the console.

## [1.4.17] - 2020-12-17
### Added
- Open error context menu on left as well as right mouse click.
- Change mouse pointer to a hand when rolling over error rows.
- Highlight error rows on mouse hover.

## [1.5.18] - 2020-12-20
### Added 
- Option to automatically check staged files whenever they change.

## [1.6.32] - 2021-06-17
### Added
- Option to ignore corrective changes. That is, changes that correct pre-existing errors in the repo.

### Fixed
- Handle cases where all of a directory's contents are renamed in a single commit.
- Display errors for GUID modifications correctly.

