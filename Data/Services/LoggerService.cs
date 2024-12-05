using Data.Helpers;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Services;

public interface ILoggerService
{
    public Task Log(string message, ThreatLvl lvl = ThreatLvl.Low);
    public Task LogMany(string[] messages, ThreatLvl lvl = ThreatLvl.Low);
    public Task<List<Log>> GetLogs();
    Task<int> GetLogsCount();
}

public class LoggerService : ILoggerService
{
    private readonly RwaContext _context;

    public LoggerService(RwaContext context)
    {
        _context = context;
    }

    public Task Log(string message, ThreatLvl lvl = ThreatLvl.Low)
    {
        var log = new Log
        {
            Lvl = lvl.ToInt(),
            Message = message,
        };

        lock (log)
        {
            _context.Logs.Add(log);
            _context.SaveChanges();
        }

        return Task.CompletedTask;
    }

    public Task LogMany(string[] messages, ThreatLvl lvl = ThreatLvl.Low)
    {
        var logs = new List<Log>(messages.Length);
        foreach (var m in messages)
        {
            logs.Add(new()
            {
                Lvl = lvl.ToInt(),
                Message = m
            });
        }

        lock (logs)
        {
            _context.Logs.AddRange(logs);
            _context.SaveChanges();
        }

        return Task.CompletedTask;
    }

    public async Task<List<Log>> GetLogs()
    {
        var logs = await _context.Logs
            .OrderByDescending(l => l.Date)
            .ToListAsync();
        return logs;
    }

    public async Task<int> GetLogsCount()
    {
        var count = await _context.Logs.CountAsync();
        return count;
    }
}