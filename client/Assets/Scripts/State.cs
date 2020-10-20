// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 0.5.41
// 

using Colyseus.Schema;

public class State : Schema {
	[Type(0, "string")]
	public string currentTurn = "";

	[Type(1, "map", "string")]
	public MapSchema<string> players = new MapSchema<string>();

	[Type(2, "array", "number")]
	public ArraySchema<float> board = new ArraySchema<float>();

	[Type(3, "string")]
	public string winner = "";

	[Type(4, "boolean")]
	public bool draw = false;
}

