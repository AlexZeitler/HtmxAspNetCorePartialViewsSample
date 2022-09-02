using AspNetPartialViewsHtmx.Contacts;
using AspNetPartialViewsHtmx.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspNetPartialViewsHtmx.Controllers;

public class ContactsController : Controller
{
  private readonly Contacts.Contacts _contacts;

  public ContactsController(Contacts.Contacts contacts)
  {
    _contacts = contacts;
  }

  public async Task<IActionResult> Index()
  {
    var model = (await _contacts.LoadContacts()).Select(c => new ContactViewModel(c)).ToList().AsReadOnly();
    return View(model);
  }

  [HttpDelete]
  [Route("/contacts/{contactId}")]
  public async Task<IActionResult> Archive([FromRoute] Guid contactId)
  {
    var contact = await _contacts.LoadContact(contactId);
    if (contact is ActiveContact)
    {
      contact = new ArchivedContact(contact as ActiveContact ?? throw new InvalidOperationException());
      await _contacts.StoreContact(contact);
    }

    return PartialView("_ArchiveUI", new ContactViewModel(contact));
  }

  [HttpPatch]
  [Route("/contacts/{contactId}/unarchive")]
  public async Task<IActionResult> Unarchive([FromRoute] Guid contactId)
  {
    var contact = await _contacts.LoadContact(contactId);
    if (contact is ArchivedContact)
    {
      contact = new ActiveContact(contact as ArchivedContact ?? throw new InvalidOperationException());
      await _contacts.StoreContact(contact);
    }

    return PartialView("_ArchiveUI", new ContactViewModel(contact));
  }
}
