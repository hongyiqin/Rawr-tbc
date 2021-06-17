This is based on a snapshot of the Rawr codebase when b16.1 was tagged.

b16.1 was the last versin of Rawr which supported TBC content.

Some clean-ups have been done to make sure the code is buildable.

Unfortunately, automatic item loading is broken as Rawr was relying on wowarmory to look up item tooltips and that option is no longer available.

Thankfully I found a version of item cache file which should have data for most (if not all) raid drops plus important dungeon blues from TBC era, you can always manually add new items if you wish to do so.

You can find the file at /PreloadItemCache/ItemCache.xml, just drop it where Rawr.exe is.

