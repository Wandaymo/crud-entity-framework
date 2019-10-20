using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WandaymoGomes.DAL;
using WandaymoGomes.Models;

namespace WandaymoGomes.Controllers
{
    public class ItensController : Controller
    {
        private LojaContexto db = new LojaContexto();

        public ActionResult Index()
        {
            return View(db.Itens.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Itens itens = db.Itens.Find(id);
            if (itens == null)
            {
                return HttpNotFound();
            }
            return View(itens);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Nome,Categoria")] Itens itens)
        {
            if (ModelState.IsValid)
            {
                db.Itens.Add(itens);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(itens);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Itens itens = db.Itens.Find(id);
            if (itens == null)
            {
                return HttpNotFound();
            }
            return View(itens);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Nome,Categoria")] Itens itens)
        {
            if (ModelState.IsValid)
            {
                //Verificação para impedir que a categoria de um item seja alterada se este estive associado a um pedido
                if (db.ItensDoPedido.Where(p => p.ItemID == itens.ID).Count() == 0)
                {
                    db.Entry(itens).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                //Caso esteja associado, apenas o nome poderá ser alterado
                else
                {
                    var itensEdit = db.Itens.FirstOrDefault(p => p.ID == itens.ID);
                    itensEdit.Nome = itens.Nome;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Itens itens = db.Itens.Find(id);
            if (itens == null)
            {
                return HttpNotFound();
            }
            return View(itens);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Uso um count para verificar se exite algum item associado a algum pedido, usando a tabela ItensDoPedido
            if (db.ItensDoPedido.Where(p => p.ItemID == id).Count() == 0)
            {
                Itens itens = db.Itens.Find(id);
                db.Itens.Remove(itens);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //Chama o método GravaDados do controller ItensDoPedido
        public ActionResult ChamaIDPC(int? itemId, int? quantidade, decimal? valor)
        {
            int? id = itemId;
            int? qtd = quantidade;
            decimal? vl = valor;
            return RedirectToAction("GravaDados", "ItensDoPedido", new
            {
                id,
                qtd,
                vl
            });
        }
    }
}
