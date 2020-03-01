using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NgHTTP.Content.Bodies;
using NgHTTP.Proxies;
using NgHTTP.Proxies.Authentication;
using NgHTTP.Requests.Impl;
using NgHTTP.Requests.Responses;
using NgHTTP.Sessions.Impl;
using NgUtil.Maths;
using Supremes.Nodes;
using TodoApi.Models;
using TodoApi.Models.Contexts;
using TodoApi.Models.DTOs;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PornhubVideoCommentItemsController : ControllerBase {
        private readonly PornhubVideoCommentContext _context;

        public PornhubVideoCommentItemsController(PornhubVideoCommentContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PornhubVideoCommentItem>>> GetTodoItems() {
            return await _context.PornhubVideoCommentItems
                .Select(x => ItemToDTO(x))
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PornhubVideoCommentItem>> GetTodoItem(long id) {
            var todoItemDTO = await _context.PornhubVideoCommentItems
                .Where(x => x.Id == id)
                .Select(x => ItemToDTO(x))
                .SingleAsync();

            if (todoItemDTO == null) {
                return NotFound();
            }

            return todoItemDTO;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodoItem(long id, PornhubVideoCommentItem todoItemDTO) {
            if (id != todoItemDTO.Id) {
                return BadRequest();
            }

            var todoItem = await _context.PornhubVideoCommentItems.FindAsync(id);
            if (todoItem == null) {
                return NotFound();
            }

            // todoItem.Name = todoItemDTO.Name;
            // todoItem.IsComplete = todoItemDTO.IsComplete;

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) when (!TodoItemExists(id)) {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<PornhubVideoCommentItem>> CreateTodoItem(PornhubVideoCommentItem item) {
            /*var todoItem = new TodoItem {
                IsComplete = todoItemDTO.IsComplete,
                Name = todoItemDTO.Name
            };

            _context.PornhubVideoCommentItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetTodoItem),
                new { id = todoItem.Id },
                ItemToDTO(todoItem));*/
            Debug.WriteLine("Logging in");

            Debug.WriteLine(item.ProxyHost);
            Debug.WriteLine(item.ProxyPort);

            Proxy proxy = new Proxy(ProxyType.Http, item.ProxyHost, item.ProxyPort, new ProxyCredentials(item.ProxyUsername, item.ProxyPassword)); 
            await Login(proxy, item.PornhubUsername, item.PornhubPassword);

            return item;
        }

        private async Task<bool> Login(Proxy proxy, string username, string password) {
            using (WebSession session = new WebSession(proxy)) {
                RequestResponse rr = session.DispatchRequest(new GetRequest("https://www.pornhub.com/login"));

                if (!rr.Validate()) {
                    Debug.WriteLine("Failed to load login page");
                    return false;
                }
                Element tokenEl = rr.GetAsDoc().Select("[name=token]").First();

                if (tokenEl is null) {
                    Debug.WriteLine("Token not found");
                    return false;
                }
                string token = tokenEl.Val;

                FormBody fb = new FormBody();
                fb.Add("loginPage", 1);
                fb.Add("redirect", "");
                fb.Add("token", token);
                fb.Add("taste_profile", "");
                fb.Add("username", username);
                fb.Add("password", password);
                fb.Add("remember_me", "on");

                rr = session.DispatchRequest(new PostRequest("https://www.pornhub.com/front/authenticate", fb));

                if (!rr.Validate()) {
                    if (rr.ResponseCode == 500) {
                        Debug.WriteLine("Blocked connection");
                        return false;
                    }
                    Debug.WriteLine("Failed to login");
                    return false;
                }
                Debug.WriteLine(rr.GetResponseContent());
                return true;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id) {
            var todoItem = await _context.PornhubVideoCommentItems.FindAsync(id);

            if (todoItem == null) {
                return NotFound();
            }

            _context.PornhubVideoCommentItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoItemExists(long id) =>
             _context.PornhubVideoCommentItems.Any(e => e.Id == id);

        private static PornhubVideoCommentItem ItemToDTO(PornhubVideoCommentItem item) {//=>
            /*new PornhubVideoCommentItem {
                Id = item.Id,
                Name = todoItem.Name,
                IsComplete = todoItem.IsComplete
            };*/
            return item;
        }
    }
}
