// @algorithm Deterministic Turing Machine Simulation (Sparse Tape)
// @category State Machine / Tape Automaton / Simulation
// @input
//   - Initial state from input header
//   - Step limit (diagnostic checksum trigger)
//   - Hard-coded transition table (state × current value → action)
// @tape-model Infinite in both directions, sparse representation
// @representation
//   - Tape: Dictionary<int, bool> (index → bit; missing = 0 / false)
//   - Head position: integer index
//   - Machine state: enum State
// @transition-rule Each step:
//   1. Read current tape value at head (default 0)
//   2. Select action by (state, value)
//   3. Write new value to tape
//   4. Move head Left or Right
//   5. Transition to next state
// @action-structure
//   (State Id, Read Value) →
//     Write bit,
//     Move direction,
//     Next state
// @lookup-strategy Linear search over action list (Single)
//   Equivalent to a transition table keyed by (state, value)
// @initial-conditions
//   - Head at position 0
//   - Tape all zeros
//   - Starting state parsed from input
// @termination Fixed number of steps (no halting state)
// @output Diagnostic checksum:
//   Count of tape cells containing 1 (true) after simulation
// @data-structures
//   - Dictionary<int, bool> for sparse tape storage
//   - Immutable Action records for transitions
//   - Enums for states and movement directions
// @complexity
//   Time: O(steps × transitions_per_lookup)
//         (≈ O(steps) since table size is constant)
//   Space: O(number_of_written_cells)
// @notes
//   - Sparse tape avoids allocating an infinite array
//   - Default false values represent blank cells
//   - Transition table encodes a deterministic machine
//   - Equivalent to Advent-style “diagnostic checksum” Turing task

await Part1();
return;

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var state = Enum.Parse<State>(lines[0][^2].ToString());
    var limit = int.Parse(lines[1].Split(' ')[5]);
    var tape = new Dictionary<int, bool>();
    var position = 0;
    Action[] actions =
    [
        new(State.A, false, true, Direction.R, State.B),
        new(State.A, true, false, Direction.R, State.F),
        new(State.B, false, false, Direction.L, State.B),
        new(State.B, true, true, Direction.L, State.C),
        new(State.C, false, true, Direction.L, State.D),
        new(State.C, true, false, Direction.R, State.C),
        new(State.D, false, true, Direction.L, State.E),
        new(State.D, true, true, Direction.R, State.A),
        new(State.E, false, true, Direction.L, State.F),
        new(State.E, true, false, Direction.L, State.D),
        new(State.F, false, true, Direction.R, State.A),
        new(State.F, true, false, Direction.L, State.E),
    ];

    for (int i = 0; i < limit; i++)
    {
        var p = tape.GetValueOrDefault(position, false);
        var act = actions.Single(a => a.Id == state && a.Value == p);
        tape[position] = act.Write;
        position += act.Direction == Direction.R ? 1 : -1;
        state = act.Next;
    }
    Console.WriteLine(tape.Values.Count(x=>x));
}

enum State { A,B,C,D,E,F }
enum Direction { L,R }

record Action(State Id, bool Value, bool Write,Direction Direction, State Next);