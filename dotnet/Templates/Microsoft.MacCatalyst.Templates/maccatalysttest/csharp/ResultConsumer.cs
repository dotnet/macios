using Microsoft.Testing.Platform.Extensions;
using Microsoft.Testing.Platform.Extensions.Messages;

[assembly: Parallelize (Scope = ExecutionScope.MethodLevel)]

namespace MacCatalystTest1;

class ResultConsumer : IDataConsumer {
	int _passed, _failed, _skipped;
	public int Passed => _passed;
	public int Failed => _failed;
	public int Skipped => _skipped;
	public string? TrxReportPath;
	public event Action<string>? StatusChanged;

	public string Uid => nameof (ResultConsumer);
	public string DisplayName => nameof (ResultConsumer);
	public string Description => "";
	public string Version => "1.0";
	public Task<bool> IsEnabledAsync () => Task.FromResult (true);

	public Type [] DataTypesConsumed => [typeof (TestNodeUpdateMessage), typeof (SessionFileArtifact)];

	public Task ConsumeAsync (IDataProducer dataProducer, IData value, CancellationToken cancellationToken)
	{
		if (value is SessionFileArtifact artifact) {
			TrxReportPath = artifact.FileInfo.FullName;

			Console.WriteLine ($"Results: passed={Passed}, failed={Failed}, skipped={Skipped}");
			Console.WriteLine ($"TRX report: {TrxReportPath}");
			StatusChanged?.Invoke ($"\n✅ {Passed} passed  ❌ {Failed} failed  ⏭️ {Skipped} skipped");
		} else if (value is TestNodeUpdateMessage { TestNode: var node }) {
			var state = node.Properties.SingleOrDefault<TestNodeStateProperty> ();
			string? outcome = state switch {
				PassedTestNodeStateProperty => "passed",
				FailedTestNodeStateProperty or ErrorTestNodeStateProperty
					or TimeoutTestNodeStateProperty => "failed",
				SkippedTestNodeStateProperty => "skipped",
				_ => null
			};
			if (outcome is null)
				return Task.CompletedTask;

			_ = outcome switch { "passed" => Interlocked.Increment (ref _passed), "failed" => Interlocked.Increment (ref _failed), _ => Interlocked.Increment (ref _skipped) };

			var id = node.Properties.SingleOrDefault<TestMethodIdentifierProperty> ();
			var testName = id is not null ? $"{id.Namespace}.{id.TypeName}.{id.MethodName}" : node.DisplayName;
			Console.WriteLine ($"[{outcome.ToUpperInvariant ()}] {testName}");

			var icon = outcome switch { "passed" => "✅", "failed" => "❌", _ => "⏭️" };
			StatusChanged?.Invoke ($"{icon} {testName}");
		}
		return Task.CompletedTask;
	}
}
