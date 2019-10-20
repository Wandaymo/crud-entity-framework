using Fare;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WandaymoGomes.DAL;
using WandaymoGomes.Models;

namespace WandaymoGomes.Controllers
{
    public class ItensDoPedidoController : Controller
    {
        private LojaContexto db = new LojaContexto();

        public ActionResult Index()
        {
            var itensDoPedido = db.ItensDoPedido.Include(i => i.Item).Include(i => i.Pedido);
            return View(itensDoPedido.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItensDoPedido itensDoPedido = db.ItensDoPedido.Find(id);
            if (itensDoPedido == null)
            {
                return HttpNotFound();
            }
            return View(itensDoPedido);
        }

        public ActionResult Create()
        {
            ViewBag.ItemID = new SelectList(db.Itens, "ID", "Nome");
            ViewBag.PedidoID = new SelectList(db.Pedidos, "ID", "Identificador");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,PedidoID,ItemID,Quantidade,Valor")] ItensDoPedido itensDoPedido)
        {
            if (ModelState.IsValid)
            {
                db.ItensDoPedido.Add(itensDoPedido);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ItemID = new SelectList(db.Itens, "ID", "Nome", itensDoPedido.ItemID);
            ViewBag.PedidoID = new SelectList(db.Pedidos, "ID", "Identificador", itensDoPedido.PedidoID);
            return View(itensDoPedido);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItensDoPedido itensDoPedido = db.ItensDoPedido.Find(id);
            if (itensDoPedido == null)
            {
                return HttpNotFound();
            }
            ViewBag.ItemID = new SelectList(db.Itens, "ID", "Nome", itensDoPedido.ItemID);
            ViewBag.PedidoID = new SelectList(db.Pedidos, "ID", "Identificador", itensDoPedido.PedidoID);
            return View(itensDoPedido);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,PedidoID,ItemID,Quantidade,Valor")] ItensDoPedido itensDoPedido)
        {
            if (ModelState.IsValid)
            {
                //Atualizo o valor total quando o valor do ItensDoPedido for alterado
                decimal? valorAntigo = 0;
                decimal? novoValor = 0;
                decimal? valorPedido = 0;
                int? quantidadeAntigo = 0;
                int? quantidadeNovo = 0;
                int? pedidoID = 0;
                pedidoID = int.Parse(db.ItensDoPedido.Where(p => p.ID == itensDoPedido.ID).Select(r => r.PedidoID).FirstOrDefault().ToString());
                valorPedido = decimal.Parse(db.Pedidos.Where(p => p.ID == pedidoID).Select(r => r.ValorTotal).FirstOrDefault().ToString());
                valorAntigo = decimal.Parse(db.ItensDoPedido.Where(p => p.ID == itensDoPedido.ID).Select(r => r.Valor).FirstOrDefault().ToString());
                novoValor = itensDoPedido.Valor;
                quantidadeNovo = itensDoPedido.Quantidade;
                quantidadeAntigo = int.Parse(db.ItensDoPedido.Where(p => p.ID == itensDoPedido.ID).Select(r => r.Quantidade).FirstOrDefault().ToString());
                var pedidosEdit = db.Pedidos.FirstOrDefault(p => p.ID == pedidoID);
                pedidosEdit.ValorTotal = valorPedido - (valorAntigo * quantidadeAntigo) + (novoValor * quantidadeNovo);
                db.SaveChanges();

                var itemEdit = db.ItensDoPedido.FirstOrDefault(p => p.ID == itensDoPedido.ID);
                itemEdit.Valor = itensDoPedido.Valor;
                itemEdit.Quantidade = itensDoPedido.Quantidade;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            ViewBag.ItemID = new SelectList(db.Itens, "ID", "Nome", itensDoPedido.ItemID);
            ViewBag.PedidoID = new SelectList(db.Pedidos, "ID", "Identificador", itensDoPedido.PedidoID);
            return View(itensDoPedido);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItensDoPedido itensDoPedido = db.ItensDoPedido.Find(id);
            if (itensDoPedido == null)
            {
                return HttpNotFound();
            }
            return View(itensDoPedido);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Atualiza o valor total do pedido quando algum item é exluído
            decimal? valorItem = 0;
            decimal? valorPedido = 0;
            int? pedidoID = 0;
            ItensDoPedido itensDoPedido = db.ItensDoPedido.Find(id);
            pedidoID = int.Parse(db.ItensDoPedido.Where(p => p.ID == id).Select(r => r.PedidoID).FirstOrDefault().ToString());
            valorPedido = decimal.Parse(db.Pedidos.Where(p => p.ID == pedidoID).Select(r => r.ValorTotal).FirstOrDefault().ToString());
            valorItem = decimal.Parse(db.ItensDoPedido.Where(p => p.ID == id).Select(r => r.Valor).FirstOrDefault().ToString());
            var pedidosEdit = db.Pedidos.FirstOrDefault(p => p.ID == pedidoID);
            pedidosEdit.ValorTotal = valorPedido -  valorItem;
            db.SaveChanges();

            db.ItensDoPedido.Remove(itensDoPedido);
            db.SaveChanges();
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

        public ActionResult GravaDados(int? id, int? qtd, decimal? vl)
        {
            if (id == null || qtd == null || vl == null)
            {
                return RedirectToAction("Index");
            }
            if(vl < 0 || qtd < 0)
            {
                return RedirectToAction("Index");
            }
            else if (id != null && qtd != null && vl != null)
            {
                if (ModelState.IsValid)
                {
                    string identificador = "P_A000_C";
                    //Uso do pacote Fare para gerar o identificador de forma aleatória seguindo o padrão "P_[**letra, seguida de 3 números e underline**]C"
                    String regex = "P_[A-Z][0-9]{3}_C";
                    Xeger generator = new Xeger(regex);
                    identificador = generator.Generate();

                    int pedidoID = 0;
                    decimal? valorTotal = 0;
                    //Eu poderia ter usado um count para verificar se há registros no banco, mas o id do primeiro registro será
                    //1, então faço uma única consulta, pois precisarei do pedidoID em seguida.
                    pedidoID = int.Parse(db.Pedidos.OrderByDescending(p => p.ID).Select(r => r.ID).FirstOrDefault().ToString());
                    //Verifica se já existe algum registro no banco
                    if (pedidoID > 0)
                    {
                        //Não cria um novo pedido, pois já existe um aberto
                        //status == 1 -> Pedido aberto
                        //status == 0 -> Pedido fechado
                        if (int.Parse(db.Pedidos.OrderByDescending(p => p.status).Select(r => r.status).FirstOrDefault().ToString()) == 1)
                        {
                            //Item com o mesmo valor já adicionado
                            if (db.ItensDoPedido.Where(p => p.ItemID == id).Where(p => p.Valor == vl).Where(p => p.PedidoID == pedidoID).Count() > 0)
                            {
                                return RedirectToAction("ErroItem");
                            }
                            else
                            { 
                                //Não cria pedido, só adiciona os itens no que já está aberto
                                ItensDoPedido ip = new ItensDoPedido { PedidoID = pedidoID, ItemID = id, Quantidade = qtd, Valor = vl };
                                db.ItensDoPedido.Add(ip);
                                db.SaveChanges();

                                //Atualiza o valor total
                                valorTotal = decimal.Parse(db.Pedidos.Where(p => p.ID == pedidoID).Select(r => r.ValorTotal).FirstOrDefault().ToString());
                                var pedidosEdit = db.Pedidos.FirstOrDefault(p => p.ID == pedidoID);
                                pedidosEdit.ValorTotal = (vl * qtd) + valorTotal;
                                db.SaveChanges();
                            }
                        }
                        //Cria um novo pedido e com o status em aberto
                        else
                        {
                            Pedidos pedidos = new Pedidos { Identificador = identificador, Descricao = "Pedido", ValorTotal = 0, status = 1 };
                            db.Pedidos.Add(pedidos);
                            db.SaveChanges();
                            pedidoID = int.Parse(db.Pedidos.OrderByDescending(p => p.ID).Select(r => r.ID).FirstOrDefault().ToString());
                            ItensDoPedido ip = new ItensDoPedido { PedidoID = pedidoID, ItemID = id, Quantidade = qtd, Valor = vl };
                            db.ItensDoPedido.Add(ip);
                            db.SaveChanges();

                            //Atualiza o valor total
                            var pedidosEdit = db.Pedidos.FirstOrDefault(p => p.ID == pedidoID);
                            pedidosEdit.ValorTotal = vl * qtd;
                            db.SaveChanges();
                        }
                    }
                    //Cria o primeiro pedido no banco
                    else
                    {
                        Pedidos pedidos = new Pedidos { Identificador = identificador, Descricao = "Pedido", ValorTotal = 0, status = 1 };
                        db.Pedidos.Add(pedidos);
                        db.SaveChanges();
                        pedidoID = int.Parse(db.Pedidos.OrderByDescending(p => p.ID).Select(r => r.ID).FirstOrDefault().ToString());
                        ItensDoPedido ip = new ItensDoPedido { PedidoID = pedidoID, ItemID = id, Quantidade = qtd, Valor = vl };
                        db.ItensDoPedido.Add(ip);
                        db.SaveChanges();

                        //Atualiza o valor total
                        valorTotal = decimal.Parse(db.Pedidos.Where(p => p.ID == pedidoID).Select(r => r.ValorTotal).FirstOrDefault().ToString());
                        var pedidosEdit = db.Pedidos.FirstOrDefault(p => p.ID == pedidoID);
                        pedidosEdit.ValorTotal = (vl * qtd) + valorTotal;
                        db.SaveChanges();
                    }
                }
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult ErroItem()
        {
            return View();
        }
    }
}
