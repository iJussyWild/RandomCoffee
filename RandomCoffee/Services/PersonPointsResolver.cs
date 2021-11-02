using System;
using System.Collections.Generic;
using System.Linq;
using RandomCoffee.Database.Entities;

namespace RandomCoffee.Services
{
	public class PersonPointsResolver
	{
		private const int IncrementInMeetings = 3;
		private const int IncrementInDepartment = 2;
		private const int IncrementInTopics = 1;

		private static readonly Random Random = new();

		private readonly Dictionary<int, Person> _personsById = new();
		private readonly Dictionary<int, int> _pointsById = new();

		public Person GetPersonForMeetingByMaxPoints(IEnumerable<Person> persons, Person targetPerson)
		{
			CalcPoints(persons, targetPerson);
			var person = GetPersonForMeetingByMaxPoints();
			Clear();

			return person;
		}

		private Person GetPersonForMeetingByMaxPoints()
		{
			var maxPoints = _pointsById.Values.Max();
			var ids = _pointsById.Where(p => p.Value == maxPoints).Select(p => p.Key).ToList();

			if (ids.Count == 1)
				return _personsById[ids[0]];

			var id = Random.Next(0, ids.Count + 1);

			return _personsById[id];
		}

		private void AddPerson(Person person)
		{
			_personsById.TryAdd(person.Id, person);
			_pointsById.TryAdd(person.Id, default);
		}

		private void Clear()
		{
			_personsById.Clear();
			_pointsById.Clear();
		}

		private void CalcPoints(IEnumerable<Person> persons, Person targetPerson)
		{
			foreach (var person in persons)
			{
				AddPerson(person);
				CalcPointsInMeetings(person, targetPerson);
				CalcPointsInDepartment(person, targetPerson);
				CalcPointsInTopics(person, targetPerson);
			}
		}

		private void CalcPointsInMeetings(Person person, Person targetPerson)
		{
			var meetingIds = person.Meetings.Select(m => m.Id).ToHashSet();
			var targetMeetingIds = targetPerson.Meetings.Select(m => m.Id).ToHashSet();
			targetMeetingIds.IntersectWith(meetingIds);

			if (targetMeetingIds.Count == 0)
				_pointsById[person.Id] += IncrementInMeetings;
		}

		private void CalcPointsInDepartment(Person person, Person targetPerson)
		{
			if (person.Department != targetPerson.Department)
				_pointsById[person.Id] += IncrementInDepartment;
		}

		private void CalcPointsInTopics(Person person, Person targetPerson)
		{
			var topicIds = person.Topics.Select(t => t.Id).ToHashSet();
			var targetTopicIds = targetPerson.Topics.Select(t => t.Id).ToHashSet();
			targetTopicIds.IntersectWith(topicIds);

			_pointsById[person.Id] += IncrementInTopics * targetTopicIds.Count;
		}
	}
}
