﻿using System.Numerics;

namespace GameUtils;
internal readonly partial struct Color
{
    /// <summary>rgb(240, 248, 255)</summary>
    public static readonly Vector3 AliceBlue = new(0.9411765f, 0.972549f, 1f);

    /// <summary>rgb(250, 235, 215)</summary>
    public static readonly Vector3 AntiqueWhite = new(0.98039216f, 0.92156863f, 0.84313726f);

    /// <summary>rgb(0, 255, 255)</summary>
    public static readonly Vector3 Aqua = new(0f, 1f, 1f);

    /// <summary>rgb(127, 255, 212)</summary>
    public static readonly Vector3 Aquamarine = new(0.49803922f, 1f, 0.83137256f);

    /// <summary>rgb(240, 255, 255)</summary>
    public static readonly Vector3 Azure = new(0.9411765f, 1f, 1f);

    /// <summary>rgb(245, 245, 220)</summary>
    public static readonly Vector3 Beige = new(0.9607843f, 0.9607843f, 0.8627451f);

    /// <summary>rgb(255, 228, 196)</summary>
    public static readonly Vector3 Bisque = new(1f, 0.89411765f, 0.76862746f);

    /// <summary>rgb(0, 0, 0)</summary>
    public static readonly Vector3 Black = new(0f, 0f, 0f);

    /// <summary>rgb(255, 235, 205)</summary>
    public static readonly Vector3 BlanchedAlmond = new(1f, 0.92156863f, 0.8039216f);

    /// <summary>rgb(0, 0, 255)</summary>
    public static readonly Vector3 Blue = new(0f, 0f, 1f);

    /// <summary>rgb(138, 43, 226)</summary>
    public static readonly Vector3 BlueViolet = new(0.5411765f, 0.16862746f, 0.8862745f);

    /// <summary>rgb(165, 42, 42)</summary>
    public static readonly Vector3 Brown = new(0.64705884f, 0.16470589f, 0.16470589f);

    /// <summary>rgb(222, 184, 135)</summary>
    public static readonly Vector3 BurlyWood = new(0.87058824f, 0.72156864f, 0.5294118f);

    /// <summary>rgb(95, 158, 160)</summary>
    public static readonly Vector3 CadetBlue = new(0.37254903f, 0.61960787f, 0.627451f);

    /// <summary>rgb(127, 255, 0)</summary>
    public static readonly Vector3 Chartreuse = new(0.49803922f, 1f, 0f);

    /// <summary>rgb(210, 105, 30)</summary>
    public static readonly Vector3 Chocolate = new(0.8235294f, 0.4117647f, 0.11764706f);

    /// <summary>rgb(255, 127, 80)</summary>
    public static readonly Vector3 Coral = new(1f, 0.49803922f, 0.3137255f);

    /// <summary>rgb(100, 149, 237)</summary>
    public static readonly Vector3 CornflowerBlue = new(0.39215687f, 0.58431375f, 0.92941177f);

    /// <summary>rgb(255, 248, 220)</summary>
    public static readonly Vector3 Cornsilk = new(1f, 0.972549f, 0.8627451f);

    /// <summary>rgb(220, 20, 60)</summary>
    public static readonly Vector3 Crimson = new(0.8627451f, 0.078431375f, 0.23529412f);

    /// <summary>rgb(0, 255, 255)</summary>
    public static readonly Vector3 Cyan = new(0f, 1f, 1f);

    /// <summary>rgb(0, 0, 139)</summary>
    public static readonly Vector3 DarkBlue = new(0f, 0f, 0.54509807f);

    /// <summary>rgb(0, 139, 139)</summary>
    public static readonly Vector3 DarkCyan = new(0f, 0.54509807f, 0.54509807f);

    /// <summary>rgb(184, 134, 11)</summary>
    public static readonly Vector3 DarkGoldenrod = new(0.72156864f, 0.5254902f, 0.043137256f);

    /// <summary>rgb(169, 169, 169)</summary>
    public static readonly Vector3 DarkGray = new(0.6627451f, 0.6627451f, 0.6627451f);

    /// <summary>rgb(0, 100, 0)</summary>
    public static readonly Vector3 DarkGreen = new(0f, 0.39215687f, 0f);

    /// <summary>rgb(189, 183, 107)</summary>
    public static readonly Vector3 DarkKhaki = new(0.7411765f, 0.7176471f, 0.41960785f);

    /// <summary>rgb(139, 0, 139)</summary>
    public static readonly Vector3 DarkMagenta = new(0.54509807f, 0f, 0.54509807f);

    /// <summary>rgb(85, 107, 47)</summary>
    public static readonly Vector3 DarkOliveGreen = new(0.33333334f, 0.41960785f, 0.18431373f);

    /// <summary>rgb(255, 140, 0)</summary>
    public static readonly Vector3 DarkOrange = new(1f, 0.54901963f, 0f);

    /// <summary>rgb(153, 50, 204)</summary>
    public static readonly Vector3 DarkOrchid = new(0.6f, 0.19607843f, 0.8f);

    /// <summary>rgb(139, 0, 0)</summary>
    public static readonly Vector3 DarkRed = new(0.54509807f, 0f, 0f);

    /// <summary>rgb(233, 150, 122)</summary>
    public static readonly Vector3 DarkSalmon = new(0.9137255f, 0.5882353f, 0.47843137f);

    /// <summary>rgb(143, 188, 139)</summary>
    public static readonly Vector3 DarkSeaGreen = new(0.56078434f, 0.7372549f, 0.54509807f);

    /// <summary>rgb(72, 61, 139)</summary>
    public static readonly Vector3 DarkSlateBlue = new(0.28235295f, 0.23921569f, 0.54509807f);

    /// <summary>rgb(47, 79, 79)</summary>
    public static readonly Vector3 DarkSlateGray = new(0.18431373f, 0.30980393f, 0.30980393f);

    /// <summary>rgb(0, 206, 209)</summary>
    public static readonly Vector3 DarkTurquoise = new(0f, 0.80784315f, 0.81960785f);

    /// <summary>rgb(148, 0, 211)</summary>
    public static readonly Vector3 DarkViolet = new(0.5803922f, 0f, 0.827451f);

    /// <summary>rgb(255, 20, 147)</summary>
    public static readonly Vector3 DeepPink = new(1f, 0.078431375f, 0.5764706f);

    /// <summary>rgb(0, 191, 255)</summary>
    public static readonly Vector3 DeepSkyBlue = new(0f, 0.7490196f, 1f);

    /// <summary>rgb(105, 105, 105)</summary>
    public static readonly Vector3 DimGray = new(0.4117647f, 0.4117647f, 0.4117647f);

    /// <summary>rgb(30, 144, 255)</summary>
    public static readonly Vector3 DodgerBlue = new(0.11764706f, 0.5647059f, 1f);

    /// <summary>rgb(178, 34, 34)</summary>
    public static readonly Vector3 FireBrick = new(0.69803923f, 0.13333334f, 0.13333334f);

    /// <summary>rgb(255, 250, 240)</summary>
    public static readonly Vector3 FloralWhite = new(1f, 0.98039216f, 0.9411765f);

    /// <summary>rgb(34, 139, 34)</summary>
    public static readonly Vector3 ForestGreen = new(0.13333334f, 0.54509807f, 0.13333334f);

    /// <summary>rgb(255, 0, 255)</summary>
    public static readonly Vector3 Fuchsia = new(1f, 0f, 1f);

    /// <summary>rgb(220, 220, 220)</summary>
    public static readonly Vector3 Gainsboro = new(0.8627451f, 0.8627451f, 0.8627451f);

    /// <summary>rgb(248, 248, 255)</summary>
    public static readonly Vector3 GhostWhite = new(0.972549f, 0.972549f, 1f);

    /// <summary>rgb(255, 215, 0)</summary>
    public static readonly Vector3 Gold = new(1f, 0.84313726f, 0f);

    /// <summary>rgb(218, 165, 32)</summary>
    public static readonly Vector3 Goldenrod = new(0.85490197f, 0.64705884f, 0.1254902f);

    /// <summary>rgb(128, 128, 128)</summary>
    public static readonly Vector3 Gray = new(0.5019608f, 0.5019608f, 0.5019608f);

    /// <summary>rgb(0, 128, 0)</summary>
    public static readonly Vector3 Green = new(0f, 0.5019608f, 0f);

    /// <summary>rgb(173, 255, 47)</summary>
    public static readonly Vector3 GreenYellow = new(0.6784314f, 1f, 0.18431373f);

    /// <summary>rgb(240, 255, 240)</summary>
    public static readonly Vector3 HoneyDew = new(0.9411765f, 1f, 0.9411765f);

    /// <summary>rgb(255, 105, 180)</summary>
    public static readonly Vector3 HotPink = new(1f, 0.4117647f, 0.7058824f);

    /// <summary>rgb(205, 92, 92)</summary>
    public static readonly Vector3 IndianRed = new(0.8039216f, 0.36078432f, 0.36078432f);

    /// <summary>rgb(75, 0, 130)</summary>
    public static readonly Vector3 Indigo = new(0.29411766f, 0f, 0.50980395f);

    /// <summary>rgb(255, 255, 240)</summary>
    public static readonly Vector3 Ivory = new(1f, 1f, 0.9411765f);

    /// <summary>rgb(240, 230, 140)</summary>
    public static readonly Vector3 Khaki = new(0.9411765f, 0.9019608f, 0.54901963f);

    /// <summary>rgb(230, 230, 250)</summary>
    public static readonly Vector3 Lavender = new(0.9019608f, 0.9019608f, 0.98039216f);

    /// <summary>rgb(255, 240, 245)</summary>
    public static readonly Vector3 LavenderBlush = new(1f, 0.9411765f, 0.9607843f);

    /// <summary>rgb(124, 252, 0)</summary>
    public static readonly Vector3 LawnGreen = new(0.4862745f, 0.9882353f, 0f);

    /// <summary>rgb(255, 250, 205)</summary>
    public static readonly Vector3 LemonChiffon = new(1f, 0.98039216f, 0.8039216f);

    /// <summary>rgb(173, 216, 230)</summary>
    public static readonly Vector3 LightBlue = new(0.6784314f, 0.84705883f, 0.9019608f);

    /// <summary>rgb(240, 128, 128)</summary>
    public static readonly Vector3 LightCoral = new(0.9411765f, 0.5019608f, 0.5019608f);

    /// <summary>rgb(224, 255, 255)</summary>
    public static readonly Vector3 LightCyan = new(0.8784314f, 1f, 1f);

    /// <summary>rgb(250, 250, 210)</summary>
    public static readonly Vector3 LightGoldenrodYellow = new(0.98039216f, 0.98039216f, 0.8235294f);

    /// <summary>rgb(211, 211, 211)</summary>
    public static readonly Vector3 LightGray = new(0.827451f, 0.827451f, 0.827451f);

    /// <summary>rgb(144, 238, 144)</summary>
    public static readonly Vector3 LightGreen = new(0.5647059f, 0.93333334f, 0.5647059f);

    /// <summary>rgb(255, 182, 193)</summary>
    public static readonly Vector3 LightPink = new(1f, 0.7137255f, 0.75686276f);

    /// <summary>rgb(255, 160, 122)</summary>
    public static readonly Vector3 LightSalmon = new(1f, 0.627451f, 0.47843137f);

    /// <summary>rgb(32, 178, 170)</summary>
    public static readonly Vector3 LightSeaGreen = new(0.1254902f, 0.69803923f, 0.6666667f);

    /// <summary>rgb(135, 206, 250)</summary>
    public static readonly Vector3 LightSkyBlue = new(0.5294118f, 0.80784315f, 0.98039216f);

    /// <summary>rgb(119, 136, 153)</summary>
    public static readonly Vector3 LightSlateGray = new(0.46666667f, 0.53333336f, 0.6f);

    /// <summary>rgb(176, 196, 222)</summary>
    public static readonly Vector3 LightSteelBlue = new(0.6901961f, 0.76862746f, 0.87058824f);

    /// <summary>rgb(255, 255, 224)</summary>
    public static readonly Vector3 LightYellow = new(1f, 1f, 0.8784314f);

    /// <summary>rgb(0, 255, 0)</summary>
    public static readonly Vector3 Lime = new(0f, 1f, 0f);

    /// <summary>rgb(50, 205, 50)</summary>
    public static readonly Vector3 LimeGreen = new(0.19607843f, 0.8039216f, 0.19607843f);

    /// <summary>rgb(250, 240, 230)</summary>
    public static readonly Vector3 Linen = new(0.98039216f, 0.9411765f, 0.9019608f);

    /// <summary>rgb(255, 0, 255)</summary>
    public static readonly Vector3 Magenta = new(1f, 0f, 1f);

    /// <summary>rgb(128, 0, 0)</summary>
    public static readonly Vector3 Maroon = new(0.5019608f, 0f, 0f);

    /// <summary>rgb(102, 205, 170)</summary>
    public static readonly Vector3 MediumAquamarine = new(0.4f, 0.8039216f, 0.6666667f);

    /// <summary>rgb(0, 0, 205)</summary>
    public static readonly Vector3 MediumBlue = new(0f, 0f, 0.8039216f);

    /// <summary>rgb(186, 85, 211)</summary>
    public static readonly Vector3 MediumOrchid = new(0.7294118f, 0.33333334f, 0.827451f);

    /// <summary>rgb(147, 112, 219)</summary>
    public static readonly Vector3 MediumPurple = new(0.5764706f, 0.4392157f, 0.85882354f);

    /// <summary>rgb(60, 179, 113)</summary>
    public static readonly Vector3 MediumSeaGreen = new(0.23529412f, 0.7019608f, 0.44313726f);

    /// <summary>rgb(123, 104, 238)</summary>
    public static readonly Vector3 MediumSlateBlue = new(0.48235294f, 0.40784314f, 0.93333334f);

    /// <summary>rgb(0, 250, 154)</summary>
    public static readonly Vector3 MediumSpringGreen = new(0f, 0.98039216f, 0.6039216f);

    /// <summary>rgb(72, 209, 204)</summary>
    public static readonly Vector3 MediumTurquoise = new(0.28235295f, 0.81960785f, 0.8f);

    /// <summary>rgb(199, 21, 133)</summary>
    public static readonly Vector3 MediumVioletRed = new(0.78039217f, 0.08235294f, 0.52156866f);

    /// <summary>rgb(25, 25, 112)</summary>
    public static readonly Vector3 MidnightBlue = new(0.09803922f, 0.09803922f, 0.4392157f);

    /// <summary>rgb(245, 255, 250)</summary>
    public static readonly Vector3 MintCream = new(0.9607843f, 1f, 0.98039216f);

    /// <summary>rgb(255, 228, 225)</summary>
    public static readonly Vector3 MistyRose = new(1f, 0.89411765f, 0.88235295f);

    /// <summary>rgb(255, 228, 181)</summary>
    public static readonly Vector3 Moccasin = new(1f, 0.89411765f, 0.70980394f);

    /// <summary>rgb(255, 222, 173)</summary>
    public static readonly Vector3 NavajoWhite = new(1f, 0.87058824f, 0.6784314f);

    /// <summary>rgb(0, 0, 128)</summary>
    public static readonly Vector3 Navy = new(0f, 0f, 0.5019608f);

    /// <summary>rgb(253, 245, 230)</summary>
    public static readonly Vector3 OldLace = new(0.99215686f, 0.9607843f, 0.9019608f);

    /// <summary>rgb(128, 128, 0)</summary>
    public static readonly Vector3 Olive = new(0.5019608f, 0.5019608f, 0f);

    /// <summary>rgb(107, 142, 35)</summary>
    public static readonly Vector3 OliveDrab = new(0.41960785f, 0.5568628f, 0.13725491f);

    /// <summary>rgb(255, 165, 0)</summary>
    public static readonly Vector3 Orange = new(1f, 0.64705884f, 0f);

    /// <summary>rgb(255, 69, 0)</summary>
    public static readonly Vector3 OrangeRed = new(1f, 0.27058825f, 0f);

    /// <summary>rgb(218, 112, 214)</summary>
    public static readonly Vector3 Orchid = new(0.85490197f, 0.4392157f, 0.8392157f);

    /// <summary>rgb(238, 232, 170)</summary>
    public static readonly Vector3 PaleGoldenrod = new(0.93333334f, 0.9098039f, 0.6666667f);

    /// <summary>rgb(152, 251, 152)</summary>
    public static readonly Vector3 PaleGreen = new(0.59607846f, 0.9843137f, 0.59607846f);

    /// <summary>rgb(175, 238, 238)</summary>
    public static readonly Vector3 PaleTurquoise = new(0.6862745f, 0.93333334f, 0.93333334f);

    /// <summary>rgb(219, 112, 147)</summary>
    public static readonly Vector3 PaleVioletRed = new(0.85882354f, 0.4392157f, 0.5764706f);

    /// <summary>rgb(255, 239, 213)</summary>
    public static readonly Vector3 PapayaWhip = new(1f, 0.9372549f, 0.8352941f);

    /// <summary>rgb(255, 218, 185)</summary>
    public static readonly Vector3 PeachPuff = new(1f, 0.85490197f, 0.7254902f);

    /// <summary>rgb(205, 133, 63)</summary>
    public static readonly Vector3 Peru = new(0.8039216f, 0.52156866f, 0.24705882f);

    /// <summary>rgb(255, 192, 203)</summary>
    public static readonly Vector3 Pink = new(1f, 0.7529412f, 0.79607844f);

    /// <summary>rgb(221, 160, 221)</summary>
    public static readonly Vector3 Plum = new(0.8666667f, 0.627451f, 0.8666667f);

    /// <summary>rgb(176, 224, 230)</summary>
    public static readonly Vector3 PowderBlue = new(0.6901961f, 0.8784314f, 0.9019608f);

    /// <summary>rgb(128, 0, 128)</summary>
    public static readonly Vector3 Purple = new(0.5019608f, 0f, 0.5019608f);

    /// <summary>rgb(102, 51, 153)</summary>
    public static readonly Vector3 RebeccaPurple = new(0.4f, 0.2f, 0.6f);

    /// <summary>rgb(255, 0, 0)</summary>
    public static readonly Vector3 Red = new(1f, 0f, 0f);

    /// <summary>rgb(188, 143, 143)</summary>
    public static readonly Vector3 RosyBrown = new(0.7372549f, 0.56078434f, 0.56078434f);

    /// <summary>rgb(65, 105, 225)</summary>
    public static readonly Vector3 RoyalBlue = new(0.25490198f, 0.4117647f, 0.88235295f);

    /// <summary>rgb(139, 69, 19)</summary>
    public static readonly Vector3 SaddleBrown = new(0.54509807f, 0.27058825f, 0.07450981f);

    /// <summary>rgb(250, 128, 114)</summary>
    public static readonly Vector3 Salmon = new(0.98039216f, 0.5019608f, 0.44705883f);

    /// <summary>rgb(244, 164, 96)</summary>
    public static readonly Vector3 SandyBrown = new(0.95686275f, 0.6431373f, 0.3764706f);

    /// <summary>rgb(46, 139, 87)</summary>
    public static readonly Vector3 SeaGreen = new(0.18039216f, 0.54509807f, 0.34117648f);

    /// <summary>rgb(255, 245, 238)</summary>
    public static readonly Vector3 SeaShell = new(1f, 0.9607843f, 0.93333334f);

    /// <summary>rgb(160, 82, 45)</summary>
    public static readonly Vector3 Sienna = new(0.627451f, 0.32156864f, 0.1764706f);

    /// <summary>rgb(192, 192, 192)</summary>
    public static readonly Vector3 Silver = new(0.7529412f, 0.7529412f, 0.7529412f);

    /// <summary>rgb(135, 206, 235)</summary>
    public static readonly Vector3 SkyBlue = new(0.5294118f, 0.80784315f, 0.92156863f);

    /// <summary>rgb(106, 90, 205)</summary>
    public static readonly Vector3 SlateBlue = new(0.41568628f, 0.3529412f, 0.8039216f);

    /// <summary>rgb(112, 128, 144)</summary>
    public static readonly Vector3 SlateGray = new(0.4392157f, 0.5019608f, 0.5647059f);

    /// <summary>rgb(255, 250, 250)</summary>
    public static readonly Vector3 Snow = new(1f, 0.98039216f, 0.98039216f);

    /// <summary>rgb(0, 255, 127)</summary>
    public static readonly Vector3 SpringGreen = new(0f, 1f, 0.49803922f);

    /// <summary>rgb(70, 130, 180)</summary>
    public static readonly Vector3 SteelBlue = new(0.27450982f, 0.50980395f, 0.7058824f);

    /// <summary>rgb(210, 180, 140)</summary>
    public static readonly Vector3 Tan = new(0.8235294f, 0.7058824f, 0.54901963f);

    /// <summary>rgb(0, 128, 128)</summary>
    public static readonly Vector3 Teal = new(0f, 0.5019608f, 0.5019608f);

    /// <summary>rgb(216, 191, 216)</summary>
    public static readonly Vector3 Thistle = new(0.84705883f, 0.7490196f, 0.84705883f);

    /// <summary>rgb(255, 99, 71)</summary>
    public static readonly Vector3 Tomato = new(1f, 0.3882353f, 0.2784314f);

    /// <summary>rgb(64, 224, 208)</summary>
    public static readonly Vector3 Turquoise = new(0.2509804f, 0.8784314f, 0.8156863f);

    /// <summary>rgb(238, 130, 238)</summary>
    public static readonly Vector3 Violet = new(0.93333334f, 0.50980395f, 0.93333334f);

    /// <summary>rgb(245, 222, 179)</summary>
    public static readonly Vector3 Wheat = new(0.9607843f, 0.87058824f, 0.7019608f);

    /// <summary>rgb(255, 255, 255)</summary>
    public static readonly Vector3 White = new(1f, 1f, 1f);

    /// <summary>rgb(245, 245, 245)</summary>
    public static readonly Vector3 WhiteSmoke = new(0.9607843f, 0.9607843f, 0.9607843f);

    /// <summary>rgb(255, 255, 0)</summary>
    public static readonly Vector3 Yellow = new(1f, 1f, 0f);

    /// <summary>rgb(154, 205, 50)</summary>
    public static readonly Vector3 YellowGreen = new(0.6039216f, 0.8039216f, 0.19607843f);
}
