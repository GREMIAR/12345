using Client.ViewModels;
using System.Collections.ObjectModel;

namespace Client.Models;

public class ViewModelPackage : ViewModelBase
{


    public ViewModelPackage()
    {
        Data = [];
        SizeGarbagePref = 256;
        garbageBytesPostfix = [];
        GarbageBytesPrefixToRandom();
        UpdateNumbers();
    }

    bool isMainPacket;


    public bool IsMainPacket
    {
        get => isMainPacket;
        set
        {
            SetProperty(ref isMainPacket, value);
            UpdateNumbers();
        }
    }

    int sizeGarbagePost;

    public int SizeGarbagePost
    {
        get => sizeGarbagePost;
        set
        {
            SetProperty(ref sizeGarbagePost, value);

            UpdateNumbersGarbageBytesPrefix();

            GarbageBytesPostfixToRandom();
            NotifyPropertyChanged(nameof(FullPackageAsString));
        }
    }

    int sizeGarbagePref;
    public int SizeGarbagePref
    {
        get => sizeGarbagePref;
        set
        {
            SetProperty(ref sizeGarbagePref, value);
            UpdateNumbersGarbageBytesPostfix();
            GarbageBytesPrefixToRandom();
            NotifyPropertyChanged(nameof(FullPackageAsString));
        }
    }

    int mainPacketBytes;

    public int MainPacketBytes
    {
        get => mainPacketBytes;
        set
        {
            SetProperty(ref mainPacketBytes, value);
            UpdateNumbers();
        }
    }

    public int MainPacketDefaultCount
    {
        get
        {
            if (IsMainPacket)
            {
                return 254 - mainPacketBytes;
            }
            else
            {
                return 256;
            }
        }
    }

    void UpdateNumbersGarbageBytesPostfix()
    {
        NotifyPropertyChanged(nameof(MainPacketDefaultCount));
        if (SizeGarbagePost + SizeGarbagePref != MainPacketDefaultCount)
        {
            SizeGarbagePost = MainPacketDefaultCount - SizeGarbagePref;
        }
        if (MainPacketDefaultCount < SizeGarbagePost)
        {
            SizeGarbagePost = MainPacketDefaultCount;
        }
    }


    void UpdateNumbersGarbageBytesPrefix()
    {
        NotifyPropertyChanged(nameof(MainPacketDefaultCount));
        if (SizeGarbagePost + SizeGarbagePref != MainPacketDefaultCount)
        {
            SizeGarbagePref = MainPacketDefaultCount - SizeGarbagePost;
        }
        if (MainPacketDefaultCount < SizeGarbagePref)
        {
            SizeGarbagePref = MainPacketDefaultCount;
        }

    }


    void UpdateNumbers()
    {
        UpdateNumbersGarbageBytesPostfix();
        UpdateNumbersGarbageBytesPrefix();
        NotifyPropertyChanged(nameof(FullPackageAsString));
    }


    byte[] garbageBytesPrefix;

    byte[] garbageBytesPostfix;

    public void GarbageBytesPrefixToRandom()
    {
        GarbageToRandom(ref garbageBytesPrefix, SizeGarbagePref);
    }

    public void GarbageBytesPostfixToRandom()
    {
        GarbageToRandom(ref garbageBytesPostfix, SizeGarbagePost);
    }

    static void GarbageToRandom(ref byte[] garbage, int max)
    {
        garbage = new byte[max];
        Random random = new();

        random.NextBytes(garbage);

        for (int i = 0; i < garbage.Length; i++)
        {
            while (garbage[i] == 10 || garbage[i] == 11)
            {
                garbage[i] = (byte)random.Next(256);
            }
        }
    }

    public void UpdateFullPackageAsString()
    {
        NotifyPropertyChanged(nameof(FullPackageAsString));
    }

    public ObservableCollection<DualByte> Data { get; set; }

    byte[] ConvertDataToByteArray
    {
        get
        {
            List<byte> byteList = Data.Select(i => i.ToByte).ToList();
            if (isMainPacket)
            {
                byteList.Insert(0, 0xA);
                byteList.Add(0xB);
            }
            return byteList.ToArray();
        }
    }

    public string FullPackageAsString
    {
        get
        {
            string fullPackage = BitConverter.ToString(garbageBytesPrefix);
            if (garbageBytesPrefix.Length > 0 && ConvertDataToByteArray.Length > 0 && IsMainPacket)
            {
                fullPackage += "-";
            }
            if (IsMainPacket)
            {
                fullPackage += BitConverter.ToString(ConvertDataToByteArray);
            }
            if (garbageBytesPostfix.Length > 0 && IsMainPacket)
            {
                fullPackage += "-";
            }
            else if (garbageBytesPostfix.Length > 0 && garbageBytesPrefix.Length > 0)
            {
                fullPackage += "-";
            }
            fullPackage += BitConverter.ToString(garbageBytesPostfix);
            return fullPackage;
        }

    }

    public byte[] FullPackage => garbageBytesPrefix.Concat(ConvertDataToByteArray).Concat(garbageBytesPostfix).ToArray();
}
