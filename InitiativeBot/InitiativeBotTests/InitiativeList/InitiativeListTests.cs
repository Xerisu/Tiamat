using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InitiativeBot.InitiativeList;
using InitiativeBot.InitiativeList.Player;
using InitiativeBot.Rolling;

using InitiativeBotTests.Rolling;

using Xunit;

namespace InitiativeBotTests.InitiativeList
{
    public class InitiativeListTests
    {
        private static readonly Player[] _mockInitiativeList = new Player[]
        {
            new Player("Seika", 101), // roll nr 2
            new Player("Kamień", 46), // roll nr 3
            new Player("Dawra", 41) // roll nr 1

        };

        [Fact]
        public void NextTurn_Should_Work()
        {
            var initiativeList = new InitiativeBot.InitiativeList.InitiativeList(_mockInitiativeList, new MockRNG());
            Assert.Equal(0, initiativeList.Round);
            Assert.Equal(0, initiativeList.ActivePlayerIndex);
            Assert.Equal(PlayerState.unactive, initiativeList.Players[0].State);
            initiativeList.NextTurn();
            Assert.Equal("Seika", initiativeList.Players[initiativeList.ActivePlayerIndex].Name);
            Assert.Equal(PlayerState.active, initiativeList.Players[initiativeList.ActivePlayerIndex].State);
            Assert.Equal(1, initiativeList.Round);
            initiativeList.NextTurn();
            Assert.Equal("Kamień", initiativeList.Players[initiativeList.ActivePlayerIndex].Name);
            Assert.Equal(PlayerState.active, initiativeList.Players[initiativeList.ActivePlayerIndex].State);
            Assert.Equal(1, initiativeList.Round);
            initiativeList.NextTurn();
            Assert.Equal("Dawra", initiativeList.Players[initiativeList.ActivePlayerIndex].Name);
            Assert.Equal(PlayerState.active, initiativeList.Players[initiativeList.ActivePlayerIndex].State);
            Assert.Equal(1, initiativeList.Round);
            initiativeList.NextTurn();
            Assert.Equal("Seika", initiativeList.Players[initiativeList.ActivePlayerIndex].Name);
            Assert.Equal(PlayerState.active, initiativeList.Players[initiativeList.ActivePlayerIndex].State);
            Assert.Equal(2, initiativeList.Round);
        }

        [Fact]
        public void ClearList_Should_Work()
        {
            var initiativeList = new InitiativeBot.InitiativeList.InitiativeList(_mockInitiativeList, new MockRNG());
            initiativeList.NextTurn();
            initiativeList.NextTurn();
            Assert.NotEqual(0, initiativeList.Round);
            Assert.NotEmpty(initiativeList.Players);
            initiativeList.ClearList();
            Assert.Equal(0, initiativeList.Round);
            Assert.Equal(0, initiativeList.ActivePlayerIndex);
            Assert.Empty(initiativeList.Players);
        }

        [Fact]
        public void RemovePlayer_Should_Work()
        {
            var initiativeList = new InitiativeBot.InitiativeList.InitiativeList(_mockInitiativeList, new MockRNG());
            initiativeList.NextTurn();
            initiativeList.NextTurn();
            Assert.Equal("Kamień", initiativeList.Players[initiativeList.ActivePlayerIndex].Name);
            initiativeList.RemovePlayer("Seika");
            Assert.Equal("Kamień", initiativeList.Players[initiativeList.ActivePlayerIndex].Name);
            Assert.Equal("Kamień", initiativeList.Players[0].Name);
            initiativeList.RemovePlayer("Kamień");
            Assert.Equal("Dawra", initiativeList.Players[initiativeList.ActivePlayerIndex].Name);
            initiativeList.NextTurn();
            Assert.Equal("Dawra", initiativeList.Players[initiativeList.ActivePlayerIndex].Name);
        }

        [Fact]
        public void RemovePlayer_Should_StartNextRoundWhenRemoveLastAndActivePlayer()
        {
            var initiativeList = new InitiativeBot.InitiativeList.InitiativeList(_mockInitiativeList, new MockRNG());
            initiativeList.NextTurn();
            initiativeList.NextTurn();
            initiativeList.NextTurn();
            Assert.Equal("Dawra", initiativeList.Players[initiativeList.ActivePlayerIndex].Name);
            initiativeList.RemovePlayer("Dawra");
            Assert.Equal("Seika", initiativeList.Players[initiativeList.ActivePlayerIndex].Name);
            Assert.Equal(2, initiativeList.Round);
        }

        [Fact]
        public void AddPlayer_Should_Work()
        {
            var initiativeList = new InitiativeBot.InitiativeList.InitiativeList(Array.Empty<Player>(), new MockRNG());
            initiativeList.AddPlayer("Dawra", new Roll(101));
            initiativeList.NextTurn();
            Assert.Equal("Dawra", initiativeList.Players[initiativeList.ActivePlayerIndex].Name);
            initiativeList.AddPlayer("Seika", new Roll(101));
            Assert.Equal("Dawra", initiativeList.Players[initiativeList.ActivePlayerIndex].Name);
            initiativeList.NextTurn();
            Assert.Equal("Seika", initiativeList.Players[initiativeList.ActivePlayerIndex].Name);
            initiativeList.AddPlayer("Kamień", new Roll(101));
            Assert.Equal("Kamień", initiativeList.Players[initiativeList.ActivePlayerIndex+1].Name);
            Assert.Equal(PlayerState.unactive, initiativeList.Players[initiativeList.ActivePlayerIndex+1].State);

        }
    }
}
