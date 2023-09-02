using Autobuses.Clases;
using Autobuses.Filters;
using Autobuses.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Autobuses.Controllers
{
    [Acceso]
    public class MarcaController : Controller
    {
        public async Task<ActionResult> Index(MarcaCLS oMarcaCLS)
        {
            List<MarcaCLS> list = new List<MarcaCLS>();

            using (var context = new BDPasajeEntities())
            {
                if (string.IsNullOrEmpty(oMarcaCLS.NOMBRE))
                {
                    list = await (from marca in context.Marca
                                  where marca.BHABILITADO == 1
                                  select new MarcaCLS()
                                  {
                                      IIDMARCA = marca.IIDMARCA,
                                      NOMBRE = marca.NOMBRE,
                                      DESCRIPCION = marca.DESCRIPCION

                                  }).ToListAsync();

                    Session["listaMarcas"] = list;
                }
                else
                {
                    list = await (from marca in context.Marca
                                  where marca.BHABILITADO == 1 && marca.NOMBRE.Contains(oMarcaCLS.NOMBRE)
                                  select new MarcaCLS()
                                  {
                                      IIDMARCA = marca.IIDMARCA,
                                      NOMBRE = marca.NOMBRE,
                                      DESCRIPCION = marca.DESCRIPCION

                                  }).ToListAsync();

                    Session["listaMarcas"] = list;
                }

            }

            return View(list);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(MarcaCLS oMarcaCLS)
        {
            bool existe = false;

            using (var bd = new BDPasajeEntities())
            {
                existe = await bd.Marca.AnyAsync(x => x.NOMBRE == oMarcaCLS.NOMBRE);
            }

            if (!ModelState.IsValid || existe)
            {
                oMarcaCLS.MensajeError = "Ya existe ese nombre en la BD..";
                return View(oMarcaCLS);
            }

            Marca marca = new Marca();
            marca.NOMBRE = oMarcaCLS.NOMBRE;
            marca.DESCRIPCION = oMarcaCLS.DESCRIPCION;
            marca.BHABILITADO = 1;

            using (var db = new BDPasajeEntities())
            {
                db.Marca.Add(marca);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            MarcaCLS oMarcaCLS = new MarcaCLS();

            using (var bd = new BDPasajeEntities())
            {
                /*oMarcaCLS = await (from marca in bd.Marca
                                   where marca.IIDMARCA == id
                                   select new MarcaCLS()
                                   {
                                        IIDMARCA = marca.IIDMARCA,
                                        NOMBRE = marca.NOMBRE,
                                        DESCRIPCION = marca.DESCRIPCION

                                   }).FirstOrDefaultAsync();*/

                Marca marca = await bd.Marca.FirstOrDefaultAsync(x => x.IIDMARCA == id);

                oMarcaCLS.IIDMARCA = marca.IIDMARCA;
                oMarcaCLS.NOMBRE = marca.NOMBRE;
                oMarcaCLS.DESCRIPCION = marca.DESCRIPCION;
            }

            return View(oMarcaCLS);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(MarcaCLS oMarcaCLS)
        {
            bool existe = false;

            using (var bd = new BDPasajeEntities())
            {
                existe = await bd.Marca.AnyAsync(x => x.NOMBRE == oMarcaCLS.NOMBRE && x.IIDMARCA != oMarcaCLS.IIDMARCA);
            }

            if (!ModelState.IsValid || existe)
            {
                oMarcaCLS.MensajeError = "Ya existe ese nombre en la BD..";
                return View(oMarcaCLS);
            }

            using (var bd = new BDPasajeEntities())
            {
                Marca marca = await bd.Marca.FirstOrDefaultAsync(x => x.IIDMARCA == oMarcaCLS.IIDMARCA);

                marca.NOMBRE = oMarcaCLS.NOMBRE;
                marca.DESCRIPCION = oMarcaCLS.DESCRIPCION;

                await bd.SaveChangesAsync();
            }


            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            using (var bd = new BDPasajeEntities())
            {
                Marca marca = await bd.Marca.FirstOrDefaultAsync(x => x.IIDMARCA == id);

                marca.BHABILITADO = 0;

                await bd.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        public FileResult Pdf()
        {
            Document doc = new Document();
            byte[] buffer;

            using (var ms = new MemoryStream())
            {
                PdfWriter.GetInstance(doc, ms);
                doc.Open();

                Paragraph title = new Paragraph("Marcas");
                title.Alignment = Element.ALIGN_CENTER;
                doc.Add(title);

                Paragraph espacio = new Paragraph(" ");
                doc.Add(espacio);

                PdfPTable table = new PdfPTable(3);
                float[] values = new float[3] { 30, 40, 80 };
                table.SetWidths(values);

                PdfPCell cell1 = new PdfPCell(new Paragraph("ID"));
                cell1.BackgroundColor = new BaseColor(130, 130, 130);
                cell1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(cell1);

                PdfPCell cell2 = new PdfPCell(new Paragraph("NOMBRE"));
                cell2.BackgroundColor = new BaseColor(130, 130, 130);
                cell2.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(cell2);

                PdfPCell cell3 = new PdfPCell(new Paragraph("DESCRIPCION"));
                cell3.BackgroundColor = new BaseColor(130, 130, 130);
                cell3.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(cell3);

                List<MarcaCLS> list = (List<MarcaCLS>)Session["listaMarcas"];

                foreach (MarcaCLS item in list)
                {
                    table.AddCell(item.IIDMARCA.ToString());
                    table.AddCell(item.NOMBRE);
                    table.AddCell(item.DESCRIPCION);
                }

                doc.Add(table);

                doc.Close();

                buffer = ms.ToArray();
            }

            return File(buffer, "application/pdf");
        }

        public FileResult Excel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            byte[] buffer;

            using (MemoryStream ms = new MemoryStream())
            {
                ExcelPackage ep = new ExcelPackage();

                ep.Workbook.Worksheets.Add("Hoja1");

                ExcelWorksheet ews = ep.Workbook.Worksheets[0];

                ews.Cells[1, 1].Value = "ID";
                ews.Cells[1, 2].Value = "NOMBRE";
                ews.Cells[1, 3].Value = "DESCRIPCION";

                ews.Column(1).Width = 20;
                ews.Column(2).Width = 40;
                ews.Column(3).Width = 180;

                using (var range = ews.Cells[1, 1, 1, 3])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Font.Color.SetColor(Color.White);
                    range.Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
                }

                List<MarcaCLS> list = (List<MarcaCLS>)Session["listaMarcas"];

                for (int i = 0; i < list.Count; i++)
                {
                    ews.Cells[i + 2, 1].Value = list[i].IIDMARCA;
                    ews.Cells[i + 2, 2].Value = list[i].NOMBRE;
                    ews.Cells[i + 2, 3].Value = list[i].DESCRIPCION;
                }

                ep.SaveAs(ms);

                buffer = ms.ToArray();
            }

            return File(buffer, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
    }
}