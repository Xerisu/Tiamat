using InitiativeBot.Commands;
using InitiativeBot.RNG;
using InitiativeBot.Rolling;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitiativeBot.InitiativeList
{
    /// <summary>
    /// Implementation of initiative list
    /// </summary>
    public class InitiativeList : IInitiativeList
    {
        private readonly List<Player.Player> _players;
        private readonly IRNG _rng;
        private int _activePlayerIndex = 0;
        private int _roundIndex = 0;

        /// <inheritdoc/>
        public int Round => _roundIndex;
        /// <inheritdoc/>
        public int ActivePlayerIndex => _activePlayerIndex;
        /// <inheritdoc/>
        public IReadOnlyList<Player.Player> Players => _players;

        public InitiativeList(IEnumerable<Player.Player> players, IRNG rng)
        {
            _players = new List<Player.Player>();
            foreach(var p in players)
                _players.Add(new Player.Player(p));
            _rng = rng;
        }

        /// <inheritdoc/>
        public void AddPlayer(string name, IRoll roll)
        {
            RemovePlayer(name);
            Player.Player player = new(name, roll.RollDice(_rng));
            _players.Add(player);
            _players.Sort();
            int addedIndex = _players.FindIndex(p => p.Name == name);
            if (addedIndex <= _activePlayerIndex) _activePlayerIndex++;
        }

        /// <inheritdoc/>
        public void ClearList()
        {
            _players.Clear();
            _roundIndex = 0;
            _activePlayerIndex = 0;
        }

        /// <inheritdoc/>
        public void ExecuteCommand(ICommand command)
        {
            command.Execute(this);
        }

        private void NextRound()
        {
            _activePlayerIndex = 0;
            _roundIndex += 1;
            _players.ForEach(p =>
            {
                if (p.State == Player.PlayerState.inactive) { p.State = Player.PlayerState.active; }
            });
        }

        /// <inheritdoc/>
        public void NextTurn()
        {
            if (_players.Count == 0) { return; }
            _activePlayerIndex++;
            while (_activePlayerIndex < _players.Count && _players[_activePlayerIndex].State != Player.PlayerState.active) { _activePlayerIndex++; }
            if (_activePlayerIndex >= _players.Count) { NextRound(); }
        }

        /// <inheritdoc/>
        public void RemovePlayer(string name)
        {
            int deletedIndex = _players.FindIndex(p => p.Name == name);
            if (deletedIndex != -1)
            {
                _players.RemoveAt(deletedIndex);
                if (deletedIndex < _activePlayerIndex) { _activePlayerIndex--; }
                if (deletedIndex == _players.Count && _activePlayerIndex == _players.Count) { NextTurn(); }
            }
        }
    }
}
