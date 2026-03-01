using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace ReplayParser
{
    public static partial class ExtensionMethods
    {
        public static ReplayInfoSet GetReplayInfoSetFromFileInfo(this FileInfo fileInfo)
        {
            ReplayInfoSet result = new ReplayInfoSet();
            using (BinaryReader fileReader = new BinaryReader(fileInfo.OpenRead()))
            {
                // [Header]
                {
                    string title = Encoding.ASCII.GetString(fileReader.ReadBytes(28));
                    uint headerSize = fileReader.ReadUInt32();
                    uint overallSize = fileReader.ReadUInt32();
                    uint replayHeaderVersion = fileReader.ReadUInt32();
                    uint overallDecompressSize = fileReader.ReadUInt32();
                    uint compressBlockCount = fileReader.ReadUInt32();

                    if (title.Contains("Warcraft III recorded game\u001a\0") == false)
                    {
                        return result;
                    }
                    result.IsReplay = true;
                    if (replayHeaderVersion <= 0)
                    {
                        return result;
                    }
                    result.IsSupport = true;

                    // [Sub-Header]
                    {
                        string w3VersionID = fileReader.ReadBytes(4)
                            .Reverse()
                            .ToArray()
                            .ToDecoding();
                        uint versionNo = fileReader.ReadUInt32();
                        ushort buildNo = fileReader.ReadUInt16();
                        ushort gameTypeFlags = fileReader.ReadUInt16();
                        TimeSpan replayLength = TimeSpan.FromMilliseconds(fileReader.ReadUInt32());
                        uint checksum = fileReader.ReadUInt32();

                        result.W3VersionID = w3VersionID;
                        result.W3VersionNo = versionNo;
                        result.W3BuildNo = buildNo;
                        result.ReplayLength = replayLength;
                    }
                }

                // [Data block header]
                {
                    uint compressedSize, decompressedSize;
                    if (result.IsReforged == false)
                    {
                        compressedSize = fileReader.ReadUInt16();
                        decompressedSize = fileReader.ReadUInt16();
                    }
                    else
                    {
                        compressedSize = fileReader.ReadUInt32();
                        decompressedSize = fileReader.ReadUInt32();
                    }
                    uint checksum = fileReader.ReadUInt32();
                    // [Decompressed data]
                    // zlib compress header skip (2 bytes)
                    fileReader.skip(2);
                    using (MemoryStream decompressedStream = fileReader.getDecompressedStream(Convert.ToInt32(compressedSize - 2)))
                    using (BinaryReader dataReader = new BinaryReader(decompressedStream))
                    {
                        // Unknown data skip (4 bytes)
                        dataReader.skip(4);
                        // [PlayerRecord]
                        {
                            byte recordID = dataReader.ReadByte();
                            byte playerID = dataReader.ReadByte();
                            string playerName = dataReader.readNullTerminatedString().ToDecoding();
                            byte additionalDataSize = dataReader.ReadByte();
                            dataReader.skip(additionalDataSize);
                        }
                        string gameRoomName = dataReader.readNullTerminatedString().ToDecoding();
                        dataReader.skip(1);

                        result.GameRoomName = gameRoomName;

                        // [Encoded String]
                        {
                            byte[] decodedBytes = dataReader
                                .readNullTerminatedString()
                                .decodedBytes();
                            // [Game Settings]
                            // ...
                            // [Map & Creator Name]
                            string rawdata = decodedBytes.ToDecoding(0x0D);
                            string[] split = rawdata.Split('\0');
                            string mapFileName = split[0];
                            string creationUserName = split[1];

                            result.MapFileName = mapFileName;
                            result.GameRoomCreationPlayerName = creationUserName;
                            result.PlayerNames.Add(creationUserName);
                        }
                        // [Player Count]
                        uint playerCount = dataReader.ReadUInt32();
                        // [Game Type]
                        {
                            byte gameType = dataReader.ReadByte();
                            byte gamePrivateFlag = dataReader.ReadByte();
                            // Unknown data skip (2 bytes)
                            dataReader.skip(2);
                        }
                        // [LeaguageID]
                        uint leaguageID = dataReader.ReadUInt32();
                        // [Player List]
                        while (true)
                        {
                            int peekRecordID = dataReader.PeekChar();
                            if (peekRecordID != 0x16)
                            {
                                break;
                            }

                            // [PlayerRecord]
                            {
                                byte recordID = dataReader.ReadByte();
                                byte playerID = dataReader.ReadByte();
                                string playerName = dataReader.readNullTerminatedString().ToDecoding();
                                byte additionalDataSize = dataReader.ReadByte();
                                dataReader.skip(additionalDataSize);
                                dataReader.skip(4);

                                result.PlayerNames.Add(playerName);
                            }
                        }
                        // [Game Start Record]
                        {
                            byte recordID = dataReader.ReadByte();
                            ushort dataSize = dataReader.ReadUInt16();
                            byte slotRecordsCount = dataReader.ReadByte();
                            // [Slot Record]
                            for (int i = 0; i < slotRecordsCount; i++)
                            {
                                byte playerID = dataReader.ReadByte();
                                byte mapDownloadPercent = dataReader.ReadByte();
                                byte slotStatus = dataReader.ReadByte();
                                byte computerPlayerFlag = dataReader.ReadByte();
                                byte teamNo = dataReader.ReadByte();
                                byte colorNo = dataReader.ReadByte();
                                byte playerRace = dataReader.ReadByte();
                                byte computerAiStrength = dataReader.ReadByte();
                                byte playerHandicap = dataReader.ReadByte();
                            }
                            uint randomSeed = dataReader.ReadUInt32();
                            byte selectMode = dataReader.ReadByte();
                            byte startSpotCount = dataReader.ReadByte();
                        }
                        // [Replay Data]
                        // ...
                    }
                }
            }
            return result;
        }

        internal static MemoryStream getDecompressedStream(this BinaryReader reader, int compressedDataSize)
        {
            using (MemoryStream src = new MemoryStream(reader.ReadBytes(compressedDataSize)))
            using (DeflateStream zlib = new DeflateStream(src, CompressionMode.Decompress))
            {
                MemoryStream result = new MemoryStream();
                zlib.CopyTo(result);
                result.Seek(0, SeekOrigin.Begin);
                return result;
            }
        }

        internal static void skip(this BinaryReader reader, int skipCount)
        {
            reader.BaseStream.Seek(skipCount, SeekOrigin.Current);
        }

        internal static string ToDecoding(this byte[] src)
        {
            return Encoding.UTF8.GetString(src);
        }

        internal static string ToDecoding(this byte[] src, int index)
        {
            return Encoding.UTF8.GetString(src, index, src.Length - index);
        }

        internal static string ToDecoding(this byte[] src, int index, int count)
        {
            return Encoding.UTF8.GetString(src, index, count);
        }

        internal static byte[] readNullTerminatedString(this BinaryReader reader)
        {
            List<byte> result = new List<byte>();
            byte readByte;
            while (true)
            {
                readByte = reader.ReadByte();
                if (readByte == 0)
                {
                    break;
                }
                result.Add(readByte);
            }
            return result.ToArray();
        }

        internal static byte[] decodedBytes(this byte[] encodedBytes)
        {
            List<byte> result = new List<byte>();
            byte mask = 0;
            for (int pos = 0; pos < encodedBytes.Length; pos++)
            {
                if (encodedBytes[pos] == 0)
                {
                    break;
                }

                if (pos % 8 == 0)
                {
                    mask = Convert.ToByte(encodedBytes[pos]);
                }
                else
                {
                    if ((mask & (0x1 << (pos % 8))) == 0)
                    {
                        result.Add(Convert.ToByte(encodedBytes[pos] - 1));
                    }
                    else
                    {
                        result.Add(Convert.ToByte(encodedBytes[pos]));
                    }
                }
            }
            return result.ToArray();
        }
    }
}
