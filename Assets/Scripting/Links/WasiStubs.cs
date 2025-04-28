using System;
using System.Diagnostics;
using Wasmtime;

namespace WasmScripting {
	public static class WasiStubs {
		public static void DefineWasiFunctions(Linker linker) {
			linker.DefineFunction("wasi_snapshot_preview1", "environ_get", (int _, int _) => 0);
			linker.DefineFunction("wasi_snapshot_preview1", "environ_sizes_get", (int environcPtr, int environsPtr) => 0);
			linker.DefineFunction("wasi_snapshot_preview1", "fd_prestat_get", (int fd, int prestatPtr) => 8);
			linker.DefineFunction("wasi_snapshot_preview1", "fd_close", (int _) => 8);
			linker.DefineFunction("wasi_snapshot_preview1", "fd_advise", (int _, long _, long _, int _) => 8);
			linker.DefineFunction("wasi_snapshot_preview1", "fd_read", (int _, int _, int _, int _) => 8);
			linker.DefineFunction("wasi_snapshot_preview1", "fd_seek", (int _, long _, int _, int _) => 8);
			linker.DefineFunction("wasi_snapshot_preview1", "fd_write", (int _, int _, int _, int _) => 8);
			linker.DefineFunction("wasi_snapshot_preview1", "fd_pread", (int _, int _, int _, long _, int _) => 8);
			linker.DefineFunction("wasi_snapshot_preview1", "fd_readdir", (int _, int _, int _, long _, int _) => 8);
			linker.DefineFunction("wasi_snapshot_preview1", "fd_filestat_get", (int _, int _) => 8);
			linker.DefineFunction("wasi_snapshot_preview1", "fd_filestat_set_size", (int _, long _) => 8);
			linker.DefineFunction("wasi_snapshot_preview1", "fd_fdstat_get", (int _, int _) => 8);
			linker.DefineFunction("wasi_snapshot_preview1", "fd_fdstat_set_flags", (int _, int _) => 8);
			linker.DefineFunction("wasi_snapshot_preview1", "fd_prestat_dir_name", (int _, int _, int _) => 8);
			linker.DefineFunction("wasi_snapshot_preview1", "path_filestat_get", (int _, int _, int _, int _, int _) => 0);
			linker.DefineFunction("wasi_snapshot_preview1", "path_readlink", (int _, int _, int _, int _, int _, int _) => 0);
			linker.DefineFunction("wasi_snapshot_preview1", "path_unlink_file", (int _, int _, int _) => 0);
			linker.DefineFunction("wasi_snapshot_preview1", "path_open", (int _, int _, int _, int _, int _, long _, long _, int _, int _) => 0);
			linker.DefineFunction("wasi_snapshot_preview1", "poll_oneoff", (int _, int _, int _, int _) => 0);
			linker.DefineFunction("wasi_snapshot_preview1", "proc_exit", (int _) => { });
			linker.DefineFunction("wasi_snapshot_preview1", "sched_yield", () => 0);
			
			linker.DefineFunction("wasi_snapshot_preview1", "clock_time_get", (Caller caller, int clockId, long precision, int timePtr) => {
				long timestamp;
				switch (clockId) {
					case 0:
						timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() * 1_000_000;
						break;
					case 1:
						timestamp = (long)(Stopwatch.GetTimestamp() * 1_000_000_000.0 / Stopwatch.Frequency);
						break;
					case 2:
						timestamp = Process.GetCurrentProcess().TotalProcessorTime.Ticks * 100;
						break;
					case 3:
						timestamp = Process.GetCurrentProcess().TotalProcessorTime.Ticks * 100;
						break;
					default:
						return 0;
				}
	    
				Memory memory = caller.GetMemory("memory")!;
				memory.WriteInt64(timePtr, timestamp);
				return 0;
			});
			
			linker.DefineFunction("wasi_snapshot_preview1", "random_get", (Caller caller, int bufPtr, int bufLen) => {
				Random random = new();
				byte[] randomBytes = new byte[bufLen];
				random.NextBytes(randomBytes);
	            
				Memory memory = caller.GetMemory("memory")!;
				for (int i = 0; i < bufLen; i++) {
					memory.WriteByte(bufPtr + i, randomBytes[i]);
				}
				return 0;
			});
		}
	}
}