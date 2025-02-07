﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using iText.Layout;
using iText.Layout.Element;
using System.IO;
using iText.Kernel.Pdf;
using OnlineBlazorApp.Data.Model;
using OnlineBlazorApp.Data.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using iText.Kernel.Colors;
using iText.Layout.Properties;

namespace OnlineBlazorApp.Data.PDF
{
    public class FacturaPDF : PageModel, IFacturaPDF
    {

        private readonly IWebHostEnvironment _env;

        public FacturaPDF(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task GeneraFactura(Factura factura, Cliente cliente, IEnumerable<DetalleFactura> detalle)
        {
            string destination = "wwwroot/FilePdf/FacturaSystem.pdf";
            FileInfo file = new FileInfo(destination);
            file.Delete();
            var fileStream = file.Create();
            fileStream.Close();
            PdfDocument pdfdoc = new PdfDocument(new PdfWriter(file));
            pdfdoc.SetTagged();

            //Escribiendo en el Documento
            using (Document document = new Document(pdfdoc))
            {
                document.Add(new Paragraph("FACTURA DE VENTA No:" + factura.IdFactura));
                document.Add(new Paragraph("Identificación del Cliente:" + cliente.IdCliente));
                document.Add(new Paragraph("Nombre del Cliente:" + cliente.NombreCliente + " " + cliente.ApellidoCliente));
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph(" DETALLE DE LA FACTURA "));
                float[] columnWidths = new float[] { 70f, 200f, 70f, 70f };
                Table table = new Table(columnWidths);
                Cell cell = new Cell(1, 1)
                   .SetBackgroundColor(ColorConstants.GRAY)
                   .SetTextAlignment(TextAlignment.CENTER)
                   .Add(new Paragraph("Código:"));
                table.AddCell(cell);
                cell = new Cell(1, 1)
                   .SetBackgroundColor(ColorConstants.GRAY)
                   .SetTextAlignment(TextAlignment.CENTER)
                   .Add(new Paragraph("Nombre Producto:"));
                table.AddCell(cell);
                cell = new Cell(1, 1)
                   .SetBackgroundColor(ColorConstants.GRAY)
                   .SetTextAlignment(TextAlignment.CENTER)
                   .Add(new Paragraph("Valor:"));
                table.AddCell(cell);
                cell = new Cell(1, 1)
                   .SetBackgroundColor(ColorConstants.GRAY)
                   .SetTextAlignment(TextAlignment.CENTER)
                   .Add(new Paragraph("Cantidad:"));
                table.AddCell(cell);
                document.Add(table);

                foreach (var aptDetalle in detalle)
                {
                    table = new Table(columnWidths);
                    table.AddCell(aptDetalle.IdProducto.ToString());
                    table.AddCell(aptDetalle.NombreProducto);
                    table.AddCell(aptDetalle.ValorProducto.ToString());
                    table.AddCell(aptDetalle.CantidadProducto.ToString());
                    document.Add(table);
                }

                document.Close();
            }

            descargarPDF();
        }

        public FileResult descargarPDF()
        {
            var filePath = Path.Combine(_env.ContentRootPath, "wwwroot/FilePdf", "FacturaSystem.pdf");
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "wwwroot/FilePdf", "FacturaSystem.pdf");
        }
    }
}
