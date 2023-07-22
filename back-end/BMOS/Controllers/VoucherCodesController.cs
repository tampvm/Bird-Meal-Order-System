using BMOS.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BMOS.Controllers
{
    public class VoucherCodesController : Controller
    {
        // GET: VoucherCodesController
        private readonly BmosContext _context;

        public VoucherCodesController(BmosContext context)
        {
            _context = context;
        }
        public ActionResult VoucherManager()
        {
            return View(_context.TblVoucherCodes.ToList());
        }



        // GET: VoucherCodesController/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: VoucherCodesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("VoucherCode, Value, Quantity")] TblVoucherCode model)
        {
            ModelState.Remove("VoucherId");
            ModelState.Remove("Used");
            ModelState.Remove("Status");
            if (ModelState.IsValid) { 
                var voucherNum = _context.TblVoucherCodes.Count(x => x.VoucherCode != null);
                voucherNum++;
                var voucher = new TblVoucherCode
                {
                    VoucherId = "voucher" + voucherNum,
                    VoucherCode = model.VoucherCode,
                    Value = model.Value,
                    Quantity = model.Quantity,
                    Used = 0,
                    Status = true,
                };
                _context.TblVoucherCodes.Add(voucher);
                _context.SaveChanges();
                return RedirectToAction(nameof(VoucherManager));
            }
            return View();
          
        }

        // GET: VoucherCodesController/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.TblVoucherCodes == null)
            {
                return NotFound();
            }

            var TblVoucherCodes = await _context.TblVoucherCodes.FindAsync(id);
            if (TblVoucherCodes == null)
            {
                return NotFound();
            }
            return View(TblVoucherCodes);
        }
        // POST: VoucherCodesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("VoucherId, VoucherCode, Value, Quantity, Used, Status")] TblVoucherCode tblVoucherCode)
        {
            if (id != tblVoucherCode.VoucherId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                _context.Update(tblVoucherCode);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(VoucherManager));
            }
            return View(tblVoucherCode);
        }

        // GET: VoucherCodesController/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null || _context.TblVoucherCodes == null)
            {
                return NotFound();
            }

            var tblVoucherCode = await _context.TblVoucherCodes
                .FirstOrDefaultAsync(m => m.VoucherId.Equals(id));
            if (tblVoucherCode == null)
            {
                return NotFound();
            }

            return View(tblVoucherCode);
        }

        // POST: VoucherCodesController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.TblVoucherCodes == null)
            {
                return Problem("Entity set 'BmosContext.TblBlogs'  is null.");
            }
            var tblVoucher = await _context.TblVoucherCodes.FindAsync(id);
            if (tblVoucher != null)
            {
                _context.TblVoucherCodes.Remove(tblVoucher);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("VoucherManager");
        }
    }
}
