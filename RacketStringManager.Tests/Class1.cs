namespace RacketStringManager.Tests
{
    // All the code in this file is included in all platforms.
    public class Class1
    {
        private readonly Job[] _jobs = new[]
            {
                new Job
                {
                    Name = "Kento Momota", Tension = 15.5, Racket = "Yonex Astrox 99", StringName = "Yonex BG65",
                    JobId = Guid.NewGuid(),IsCompleted = false, IsPaid = false,
                    Comment = "Lorem ipsum dolor sit amet", StartDate = DateOnly.FromDateTime(DateTime.Today)
                },
                new Job
                {
                    Name = "Kento Momota", Tension = 15.5, Racket = "Yonex Astrox 99", StringName = "Yonex BG65",
                    JobId = Guid.NewGuid(),IsCompleted = true, IsPaid = true,
                    Comment = "Lorem ipsum dolor sit amet", StartDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-18))
                },
                new Job
                {
                    Name = "Kento Momota", Tension = 13.5, Racket = "Yonex Astrox 99", StringName = "Yonex BG80",
                    JobId = Guid.NewGuid(),IsCompleted = true, IsPaid = true,
                    Comment = "Lorem ipsum dolor sit amet", StartDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-32))
                },
                new Job
                {
                    Name = "Kento Momota", Tension = 15, Racket = "Yonex DUORA Z STRIKE", StringName = "Yonex BG80",
                    JobId = Guid.NewGuid(),IsCompleted = true, IsPaid = true,
                    Comment = "Lorem ipsum dolor sit amet", StartDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-102))
                },
                new Job
                {
                    Name = "Kento Momota", Tension = 15, Racket = "Yonex DUORA Z STRIKE", StringName = "Yonex BG65",
                    JobId = Guid.NewGuid(),IsCompleted = true, IsPaid = true,
                    Comment = "Lorem ipsum dolor sit amet", StartDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-132))
                },
                new Job
                {
                    Name = "Viktor Axelsen", Tension = 18.5, Racket = "Yonex Astrox 100 ZZ", StringName = "Yonex BG80",
                    JobId = Guid.NewGuid(),IsCompleted = false, IsPaid = false,
                    Comment = "Lorem ipsum dolor sit amet", StartDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-2))
                },
                new Job
                {
                    Name = "Viktor Axelsen", Tension = 15.5, Racket = "Yonex Astrox 100 ZZ", StringName = "Yonex BG80",
                    JobId = Guid.NewGuid(),IsCompleted = true, IsPaid = true,
                    Comment = "Lorem ipsum dolor sit amet", StartDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-12))
                },
                new Job
                {
                    Name = "Viktor Axelsen", Tension = 13.5, Racket = "Yonex Astrox 100 ZZ", StringName = "Yonex BG80",
                    JobId = Guid.NewGuid(),IsCompleted = true, IsPaid = true,
                    Comment = "Lorem ipsum dolor sit amet", StartDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-22))
                },
                new Job
                {
                    Name = "Mark Lamsfuß", Tension = 18, Racket = "Yonex Astrox 88S", StringName = "Yonex Aerobite",
                    JobId = Guid.NewGuid(),IsCompleted = false, IsPaid = true,
                    Comment = "Lorem ipsum dolor sit amet", StartDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-10))
                },
                new Job
                {
                    Name = "Mark Lamsfuß", Tension = 17.5, Racket = "Yonex Astrox 88S", StringName = "Yonex Aerobite",
                    JobId = Guid.NewGuid(),IsCompleted = true, IsPaid = true,
                    Comment = "Lorem ipsum dolor sit amet", StartDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-24))
                },
                new Job
                {
                    Name = "Mark Lamsfuß", Tension = 18, Racket = "Yonex Astrox 88S", StringName = "Yonex Aerobite",
                    JobId = Guid.NewGuid(),IsCompleted = true, IsPaid = true,
                    Comment = "Lorem ipsum dolor sit amet", StartDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-34))
                },
                new Job
                {
                    Name = "Mark Lamsfuß", Tension = 17.5, Racket = "Yonex Astrox 88S", StringName = "Yonex Aerobite",
                    JobId = Guid.NewGuid(),IsCompleted = true, IsPaid = true,
                    Comment = "Lorem ipsum dolor sit amet", StartDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-44))
                },
                new Job
                {
                    Name = "Mark Lamsfuß", Tension = 16, Racket = "Yonex Astrox 88S", StringName = "Yonex Aerobite",
                    JobId = Guid.NewGuid(),IsCompleted = true, IsPaid = true,
                    Comment = "Lorem ipsum dolor sit amet", StartDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-64))
                },
            };

        private JobRepository _repository;
        
        [SetUp]
        public void Setup()
        {
            _repository = new JobRepository();
            _repository.Clear();
            foreach (var job in _jobs)
            {
                _repository.Create(job);
            }
        }

        [TearDown]
        public void TearDown()
        {
            _repository.Dispose();
        }

        [Test]
        public void T01()
        {
            _repository.Clear();
            foreach (var job in _jobs)
            {
                _repository.Create(job);
            }

            var all = _repository.GetAllJobs();

            Assert.AreEqual(_jobs.Length, all.Count());
        }

        [Test]
        public async  Task T02()
        {
            //_repository.Clear();
            //foreach (var job in _jobs)
            //{
            //    _repository.Update(new JobEntity(job));
            //}

            //var all = await _repository.GetAllJobs();

            //Assert.AreEqual(0, all.Count());
        }

        [Test]
        public void FindById()
        {
            var expected = _repository.GetAllJobs().FirstOrDefault();
            Assert.IsNotNull(expected);

            var actual = _repository.Find(expected.JobId);

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Tension, actual.Tension);
        }

        [Test]
        public void DeleteById()
        {
            var expected = _repository.GetAllJobs().FirstOrDefault();
            Assert.IsNotNull(expected);

            _repository.Delete(expected);
            
            Assert.IsNull(_repository.Find(expected.JobId));
        }

        [Test]
        public void GetAllPlayers()
        {
            var expected = _jobs.Select(x => x.Name).Distinct();

            var actual = _repository.GetAllPlayerNames();
            
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void GetAllStrings()
        {
            var expected = _jobs.Select(x => x.StringName).Distinct();

            var actual = _repository.GetAllStringNames();
            
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void GetAllRacketsForPlayer()
        {
            var players = _jobs.Select(x => x.Name).Distinct();

            foreach (var player in players)
            {
                var expected = _jobs
                    .Where(x => x.Name == player)
                    .Select(x => x.Racket).Distinct();

                var actual = _repository.GetAllRacketsForPlayer(player);

                CollectionAssert.AreEquivalent(expected, actual, "unexpected racket for player " + player);
            }
        }
    }
}
