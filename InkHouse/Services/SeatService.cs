using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using InkHouse.Models;

namespace InkHouse.Services
{
    /// <summary>
    /// åº§ä½æœåŠ¡ç±»ï¼Œè´Ÿè´£åº§ä½é¢„çº¦ç›¸å…³ä¸šåŠ¡é€»è¾‘
    /// </summary>
    public class SeatService
    {
        private readonly DbContextFactory _dbContextFactory;

        public SeatService(DbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        /// <summary>
        /// è·å–æ‰€æœ‰åº§ä½
        /// </summary>
        public async Task<List<Seat>> GetAllSeatsAsync()
        {
            using var context = _dbContextFactory.CreateDbContext();
            return await context.Seats.AsNoTracking().OrderBy(s => s.SeatNumber).ToListAsync();
        }

        /// <summary>
        /// è·å–åº§ä½é¢„çº¦ç»Ÿè®¡ä¿¡æ¯
        /// </summary>
        public async Task<(int totalSeats, int freeSeats, int reservedSeats, int occupiedSeats)> GetSeatStatisticsAsync()
        {
            using var context = _dbContextFactory.CreateDbContext();
            var totalSeats = await context.Seats.CountAsync();
            var freeSeats = await context.Seats.CountAsync(s => s.Status == "Free");
            var reservedSeats = await context.Seats.CountAsync(s => s.Status == "Reserved");
            var occupiedSeats = await context.Seats.CountAsync(s => s.Status == "Occupied");
            return (totalSeats, freeSeats, reservedSeats, occupiedSeats);
        }

        /// <summary>
        /// ç”¨æˆ·é¢„çº¦åº§ä½
        /// </summary>
        public async Task<SeatReservation> ReserveSeatAsync(int seatId, int userId)
        {
            using var context = _dbContextFactory.CreateDbContext();
            var seat = await context.Seats.FindAsync(seatId);
            if (seat == null)
                throw new InvalidOperationException("åº§ä½ä¸å­˜åœ¨");
            if (seat.Status != "Free")
                throw new InvalidOperationException("è¯¥åº§ä½ä¸å¯é¢„çº¦");
            // æ£€æŸ¥ç”¨æˆ·æ˜¯å¦å·²æœ‰æœªå®Œæˆçš„é¢„çº¦
            bool hasActive = await context.SeatReservations.AnyAsync(r => r.UserId == userId && (r.Status == "å·²é¢„çº¦" || r.Status == "ä½¿ç”¨ä¸­"));
            if (hasActive)
                throw new InvalidOperationException("æ‚¨å·²æœ‰æœªå®Œæˆçš„åº§ä½é¢„çº¦");
            // åˆ›å»ºé¢„çº¦è®°å½•
            var reservation = new SeatReservation
            {
                UserId = userId,
                SeatId = seatId,
                ReserveTime = DateTime.Now,
                Status = "å·²é¢„çº¦"
            };
            context.SeatReservations.Add(reservation);
            // æ›´æ–°åº§ä½çŠ¶æ€
            seat.Status = "Reserved";
            seat.CurrentUserId = userId;
            await context.SaveChangesAsync();
            return reservation;
        }

        /// <summary>
        /// ç”¨æˆ·åˆ°é¦†ï¼Œåº§ä½å˜ä¸ºä½¿ç”¨ä¸­
        /// </summary>
        public async Task<SeatReservation> CheckInAsync(int reservationId)
        {
            using var context = _dbContextFactory.CreateDbContext();
            var reservation = await context.SeatReservations.Include(r => r.Seat).FirstOrDefaultAsync(r => r.Id == reservationId);
            if (reservation == null)
                throw new InvalidOperationException("é¢„çº¦è®°å½•ä¸å­˜åœ¨");
            if (reservation.Status != "å·²é¢„çº¦")
                throw new InvalidOperationException("å½“å‰é¢„çº¦ä¸å¯åˆ°é¦†");
            reservation.Status = "ä½¿ç”¨ä¸­";
            reservation.CheckInTime = DateTime.Now;
            // æ›´æ–°åº§ä½çŠ¶æ€
            if (reservation.Seat != null)
            {
                reservation.Seat.Status = "Occupied";
            }
            await context.SaveChangesAsync();
            return reservation;
        }

        /// <summary>
        /// ç”¨æˆ·ç¦»é¦†ï¼Œåº§ä½å˜ä¸ºç©ºé—²
        /// </summary>
        public async Task<SeatReservation> CheckOutAsync(int reservationId)
        {
            using var context = _dbContextFactory.CreateDbContext();
            var reservation = await context.SeatReservations.Include(r => r.Seat).FirstOrDefaultAsync(r => r.Id == reservationId);
            if (reservation == null)
                throw new InvalidOperationException("é¢„çº¦è®°å½•ä¸å­˜åœ¨");
            if (reservation.Status != "ä½¿ç”¨ä¸­")
                throw new InvalidOperationException("å½“å‰é¢„çº¦ä¸å¯ç¦»é¦†");
            reservation.Status = "å·²ç¦»é¦†";
            reservation.CheckOutTime = DateTime.Now;
            // æ›´æ–°åº§ä½çŠ¶æ€
            if (reservation.Seat != null)
            {
                reservation.Seat.Status = "Free";
                reservation.Seat.CurrentUserId = null;
            }
            await context.SaveChangesAsync();
            return reservation;
        }

        /// <summary>
        /// è·å–ç”¨æˆ·å½“å‰æœ‰æ•ˆé¢„çº¦ï¼ˆå·²é¢„çº¦/ä½¿ç”¨ä¸­ï¼‰
        /// </summary>
        public async Task<SeatReservation?> GetUserActiveReservationAsync(int userId)
        {
            using var context = _dbContextFactory.CreateDbContext();
            return await context.SeatReservations
                .Include(r => r.Seat)
                .Where(r => r.UserId == userId && (r.Status == "å·²é¢„çº¦" || r.Status == "ä½¿ç”¨ä¸­"))
                .OrderByDescending(r => r.ReserveTime)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// è·å–æ‰€æœ‰åº§ä½é¢„çº¦æƒ…å†µ
        /// </summary>
        public async Task<List<SeatReservation>> GetAllActiveReservationsAsync()
        {
            using var context = _dbContextFactory.CreateDbContext();
            return await context.SeatReservations
                .Include(r => r.Seat)
                .Include(r => r.User)
                .Where(r => r.Status == "å·²é¢„çº¦" || r.Status == "ä½¿ç”¨ä¸­")
                .ToListAsync();
        }

        /// <summary>
        /// »ñÈ¡ÓÃ»§ËùÓĞ×ùÎ»Ô¤Ô¼¼ÇÂ¼
        /// </summary>
        public async Task<List<SeatReservation>> GetUserReservationsAsync(int userId)
        {
            try
            {
                Console.WriteLine($"¿ªÊ¼²éÑ¯ÓÃ»§ID {userId} µÄ×ùÎ»Ô¤Ô¼¼ÇÂ¼...");
                using var context = _dbContextFactory.CreateDbContext();
                
                // ÏÈ¼ì²éÊı¾İ¿âÖĞÊÇ·ñÓĞÊı¾İ
                var totalCount = await context.SeatReservations.CountAsync();
                Console.WriteLine($"Êı¾İ¿âÖĞ×Ü¹²ÓĞ {totalCount} Ìõ×ùÎ»Ô¤Ô¼¼ÇÂ¼");
                
                var userCount = await context.SeatReservations.CountAsync(sr => sr.UserId == userId);
                Console.WriteLine($"ÓÃ»§ {userId} ÔÚÊı¾İ¿âÖĞÓĞ {userCount} Ìõ×ùÎ»Ô¤Ô¼¼ÇÂ¼");
                
                var records = await context.SeatReservations
                    .Include(r => r.Seat)
                    .Where(r => r.UserId == userId)
                    .OrderByDescending(r => r.ReserveTime)
                    .ToListAsync();
                
                Console.WriteLine($"²éÑ¯µ½ {records.Count} Ìõ×ùÎ»Ô¤Ô¼¼ÇÂ¼");
                foreach (var record in records)
                {
                    Console.WriteLine($"×ùÎ»Ô¤Ô¼¼ÇÂ¼: ID={record.Id}, ÓÃ»§ID={record.UserId}, ×ùÎ»ID={record.SeatId}, ×´Ì¬={record.Status}");
                }
                
                return records;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"²éÑ¯ÓÃ»§×ùÎ»Ô¤Ô¼¼ÇÂ¼Ê±·¢ÉúÒì³£: {ex.Message}");
                Console.WriteLine($"Òì³£¶ÑÕ»: {ex.StackTrace}");
                return new List<SeatReservation>();
            }
        }

        /// <summary>
        /// Ìí¼Ó×ùÎ»
        /// </summary>
        public async Task<Seat> AddSeatAsync(string seatNumber)
        {
            using var context = _dbContextFactory.CreateDbContext();
            if (await context.Seats.AnyAsync(s => s.SeatNumber == seatNumber))
                throw new InvalidOperationException("×ùÎ»±àºÅÒÑ´æÔÚ");
            var seat = new Seat { SeatNumber = seatNumber, Status = "Free" };
            context.Seats.Add(seat);
            await context.SaveChangesAsync();
            return seat;
        }

        /// <summary>
        /// É¾³ı×ùÎ»
        /// </summary>
        public async Task<bool> DeleteSeatAsync(int seatId)
        {
            using var context = _dbContextFactory.CreateDbContext();
            var seat = await context.Seats.FindAsync(seatId);
            if (seat == null) return false;
            context.Seats.Remove(seat);
            await context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// ĞŞ¸Ä×ùÎ»×´Ì¬
        /// </summary>
        public async Task<Seat> UpdateSeatStatusAsync(int seatId, string newStatus)
        {
            using var context = _dbContextFactory.CreateDbContext();
            var seat = await context.Seats.FindAsync(seatId);
            if (seat == null) throw new InvalidOperationException("×ùÎ»²»´æÔÚ");
            seat.Status = newStatus;
            if (newStatus == "Free") seat.CurrentUserId = null;
            await context.SaveChangesAsync();
            return seat;
        }
    }
} 