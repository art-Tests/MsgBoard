using MsgBoard.ViewModel.Member;

namespace MsgBoard.Services
{
    internal class MemberService
    {
        public void CreateMember(MemberCreateViewModel model, string savePath)
        {
            if (model.File.ContentLength > 0)
            {
                model.File.SaveAs(savePath);
            }
        }
    }
}