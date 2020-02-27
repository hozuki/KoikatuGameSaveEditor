# Koikatu Game Save Editor

A simple [Koikatu](http://www.illusion.jp/preview/koikatu/index.php) (コイカツ！) save game editor written in C#.

Save data structure is based on [KoikatuSaveDataEdit](https://github.com/kiletw/KoikatuSaveDataEdit).

[Downloads](https://github.com/hozuki/KoikatuGameSaveEditor/releases)

## Features

- Reading/modifying some properties in the save, obviously.
- Supports partial translation.

Still, there are hardcoded strings. And, there are, some unimplemented functions. But it should have covered the most common use cases.

## Requirements

- .NET Framework 4.7.2 or above
- Windows (may run on Wine)

Note: I planned to use WinForms for .NET Core but it is not stable yet. Distribution is also a concern.

## Challenges

The data structure is quite a mess. So I tried to clean them up and provide a simple, neat interface for end users, as always.

[`FlowLayoutPanel`](https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.flowlayoutpanel) is suitable for this card-style display. However, it suffers from low performance. I had to write a simplified version for this specific use case, with efficiency improvements. Now you still have to wait for ~10 seconds for opening a save of maximum (38) students; scrolling is a bit laggy. But it is already better than before.
