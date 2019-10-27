namespace movement_simulator
{
    public static class AnimalStatic
    {
        public static Animal[] Get()
        {
            return new[]
            {
                new Animal
                {
                    AnimalId = 0,
                    Birthdate = 1572136851,
                    Coordinate = new Coordinate(49, 19),
                    Location = "Hungary",
                    Name = "Leo",
                    Owner = "JunctionX",
                    Type = "Harpy Eagle",
                    ImageUrl = "bird_marker1",
                    PhotoUrls = new []
                    {
                        "https://firebasestorage.googleapis.com/v0/b/tracksy-5a1ad.appspot.com/o/52929241%403x.png?alt=media&token=8f253b1a-ff8f-445a-8ce3-71ec98f84aa5",
                        "https://firebasestorage.googleapis.com/v0/b/tracksy-5a1ad.appspot.com/o/harpyEagle%403x.png?alt=media&token=bdac1e65-c4da-4896-98a6-5347fb0c2d61",
                        "https://firebasestorage.googleapis.com/v0/b/tracksy-5a1ad.appspot.com/o/harpy%403x.png?alt=media&token=18c7dd73-947d-4483-a0af-57cc512cfb1e"
                    }
                },
                new Animal
                {
                    AnimalId = 1,
                    Birthdate = 1572136851,
                    Coordinate = new Coordinate(43, 19),
                    Location = "Hungary",
                    Name = "Google",
                    Type = "Harpy Eagle",
                    ImageUrl = "bird_marker2",
                    PhotoUrls = new []
                    {
                        "https://firebasestorage.googleapis.com/v0/b/tracksy-5a1ad.appspot.com/o/52929241%403x.png?alt=media&token=8f253b1a-ff8f-445a-8ce3-71ec98f84aa5",
                        "https://firebasestorage.googleapis.com/v0/b/tracksy-5a1ad.appspot.com/o/harpyEagle%403x.png?alt=media&token=bdac1e65-c4da-4896-98a6-5347fb0c2d61",
                        "https://firebasestorage.googleapis.com/v0/b/tracksy-5a1ad.appspot.com/o/maxresdefault%403x.png?alt=media&token=0255d173-c767-497f-9f45-e0e40fc51c18"
                    }
                },
                new Animal
                {
                    AnimalId = 2,
                    Birthdate = 1572136851,
                    Coordinate = new Coordinate(40, 19),
                    Location = "Hungary",
                    Name = "Leo",
                    Type = "Harpy Eagle",
                    ImageUrl = "bird_marker3",
                    PhotoUrls = new []
                    {
                        "https://firebasestorage.googleapis.com/v0/b/tracksy-5a1ad.appspot.com/o/52929241%403x.png?alt=media&token=8f253b1a-ff8f-445a-8ce3-71ec98f84aa5",
                        "https://firebasestorage.googleapis.com/v0/b/tracksy-5a1ad.appspot.com/o/harpyEagle%403x.png?alt=media&token=bdac1e65-c4da-4896-98a6-5347fb0c2d61",
                        "https://firebasestorage.googleapis.com/v0/b/tracksy-5a1ad.appspot.com/o/harpy%403x.png?alt=media&token=18c7dd73-947d-4483-a0af-57cc512cfb1e"
                    }
                }
            };
        }
    }
}