using Sandbox;

namespace MinimalExample;

public static class ConsoleCommands
{
	[ConVar.Replicated( "glizzy_god" )] 
	public static int GlizzyGod { get; set; } = 0;
}
