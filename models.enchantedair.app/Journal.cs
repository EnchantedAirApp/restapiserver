using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace app.enchantedair.model
{
    public enum Auth0Provider
    {
        googleoauth2 = 0
    }

    public record User(
        [property: Key,
        DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        int Id,
        string FirstName,
        string LastName,
        string Email,
        DateTime DateOfBirth,
        bool EmailVerified,
        DateTime CreateDate,
        DateTime UpdatedDate,
        Auth0Provider Auth0OATHProvider,
        string Auth0ID,
        SystemRole Role,
        RecordState State);

    public record Mood(

        int Id,
        Emotions Emotion,
        int Intensity,
        int User,
        DateTime Created,
        DateTime Updated,
        RecordState State);

    [Flags]
    public enum RecordState
    {
        Active = 0,
        Deleted = 1
    }

    [Flags]
    public enum SystemRole
    {
        User = 0,
        Admin = 1
    }

    public enum Emotions
    {
        Joy = 0,
        Sad = 1,
        Anger = 2,
        Disgust = 3,
        Surprise = 4,
        Fear = 5,
        Guilt = 6,
        Love = 7,
        Shame = 8,
        Embarrassment = 9,
        Hope = 10,
        Gratitude = 11,
        Jealousy = 12,
        Pride = 13,
        Compassion = 14,
        Relief = 15,
        Awe = 16,
        Anticipation = 17,
        Nostalgia = 18,
        Bitterness = 19,
        Disappointment = 20,
        Frustration = 21,
        Curiosity = 22,
        Boredom = 23,
        Confusion = 24,
        Trust = 25,
        Contempt = 26
    }
}