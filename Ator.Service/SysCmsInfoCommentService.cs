using Ator.Entity.Sys;
using Ator.IService;
using Ator.Model.Api.Comment;
using Ator.Repository;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Text;
using Ator.Utility.Ext;
using System.Linq;
namespace Ator.Service
{
    public class SysCmsInfoCommentService : ISysCmsInfoCommentService
    {
        UnitOfWork _unitOfWork;
        public SysCmsInfoCommentService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 获取评论所需要的Json
        /// </summary>
        /// <param name="SysCmsInfoId"></param>
        /// <param name="SysCmsColumnId"></param>
        /// <param name="Page"></param>
        /// <param name="Limit"></param>
        /// <returns></returns>
        public List<Comment> GetComments(string SysCmsInfoId,string SysCmsColumnId,int Page,int Limit,out long Total)
        {
            var predicate = PredicateBuilder.New<SysCmsInfoComment>(true);//查询条件
            predicate.And(o => o.Status == 1);
            predicate.And(o => string.IsNullOrEmpty(o.ToCommentId));//不包含二级评论
            if (SysCmsInfoId == null)
            {
                predicate.And(o => string.IsNullOrEmpty(o.SysCmsInfoId));
            }
            else if(SysCmsInfoId != "")
            {
                predicate.And(o => o.SysCmsInfoId == SysCmsInfoId);
            }
            if (SysCmsColumnId == null)
            {
                predicate.And(o => string.IsNullOrEmpty(o.SysCmsColumnId));
            }
            else if (SysCmsColumnId != "")
            {
                predicate.And(o => o.SysCmsColumnId == SysCmsColumnId);
            }
            //分页获取1级评论
            var conments = _unitOfWork.SysCmsInfoCommentRepository.GetPage(predicate, "-CommentTime", Page, Limit);
            Total = conments.Totals;
            //构造json数据,先不考虑数据库效率问题
            List <Comment> lstComments = new List<Comment>();
            foreach (var comment in conments.Rows)
            {
                var commentUser =  _unitOfWork.SysUserRepository.Get(comment.SysUserId) ;
                var formartComment = new Comment()
                {
                    article = SysCmsInfoId,
                    commentDate = comment.CommentTime.ToDateTimeString(),
                    commentId = comment.SysCmsInfoCommentId,
                    content = comment.Comment,
                    reply = new List<Reply>(),
                    site = comment.Address,
                    user = new CommentUser
                    {
                        commentNum = 0,
                        headPortrait = commentUser?.Avatar,
                        latelyLoginTime = comment.CommentTime.ToDateTimeString(),
                        registrationDate = comment.CommentTime.ToDateTimeString(),
                        nickname = commentUser.NikeName,
                        sex = commentUser.Sex,
                        userId = commentUser.SysUserId,//博主的userId='admin'
                        userType = commentUser.UserType
                    },
                };
                //判断是否存在子评论
                var reComments = _unitOfWork.SysCmsInfoCommentRepository.Load(o => o.Status == 1 && o.ToCommentId == comment.SysCmsInfoCommentId).OrderByDescending(o => o.CommentTime).ToList();
                //循环添加子评论
                foreach (var reComment in reComments)
                {
                    var reCommentUser = _unitOfWork.SysUserRepository.Get(reComment.SysUserId);
                    formartComment.reply.Add(new Reply
                    {
                        replyId = reComment.SysCmsInfoCommentId,
                        comment = new ReplyComment
                        {
                            article = formartComment.article,
                            commentDate = formartComment.commentDate,
                            commentId = formartComment.commentId,
                            site = formartComment.site,
                            content = formartComment.content,
                            user = formartComment.user
                        },
                        user = new CommentUser
                        {
                            commentNum = 0,
                            headPortrait = reCommentUser?.Avatar,
                            latelyLoginTime = reComment.CommentTime.ToDateTimeString(),
                            registrationDate = reComment.CommentTime.ToDateTimeString(),
                            nickname = reCommentUser.NikeName,
                            sex = reCommentUser.Sex,
                            userId = reCommentUser.SysUserId,//博主的userId='admin'
                            userType = reCommentUser.UserType
                        },
                        content = reComment.Comment,
                        replyDate = reComment.CommentTime.ToDateTimeString(),
                        site = reComment.Address
                    });
                }
                lstComments.Add(formartComment);
            }
            return lstComments;
        }

        public Comment GetComment(string SysCmsInfoCommentId)
        {
            var comment = _unitOfWork.SysCmsInfoCommentRepository.Get(SysCmsInfoCommentId);
            
            if (comment == null) return new Comment();

            var commentUser = _unitOfWork.SysUserRepository.Get(comment.SysUserId);
            var formartComment = new Comment()
            {
                article = comment.SysCmsInfoId,
                commentDate = comment.CommentTime.ToDateTimeString(),
                commentId = comment.SysCmsInfoCommentId,
                content = comment.Comment,
                reply = new List<Reply>(),
                site = comment.Address,
                user = new CommentUser
                {
                    commentNum = 0,
                    headPortrait = commentUser?.Avatar,
                    latelyLoginTime = comment.CommentTime.ToDateTimeString(),
                    registrationDate = comment.CommentTime.ToDateTimeString(),
                    nickname = commentUser.NikeName,
                    sex = commentUser.Sex,
                    userId = commentUser.SysUserId,//博主的userId='admin'
                    userType = commentUser.UserType
                },
            };
            return formartComment;
        }

        public Reply GetReply(string SysCmsInfoCommentId)
        {
            var reply = _unitOfWork.SysCmsInfoCommentRepository.Get(SysCmsInfoCommentId);
            if (reply == null) return new Reply();
            var comment = _unitOfWork.SysCmsInfoCommentRepository.Get(reply.ToCommentId);
            var commentUser = _unitOfWork.SysUserRepository.Get(comment.SysUserId);
            var replyUser = _unitOfWork.SysUserRepository.Get(reply.SysUserId);

            var formartReplyComment = new ReplyComment()
            {
                article = comment.SysCmsInfoId,
                commentDate = comment.CommentTime.ToDateTimeString(),
                commentId = comment.SysCmsInfoCommentId,
                content = comment.Comment,
                site = comment.Address,
                user = new CommentUser
                {
                    commentNum = 0,
                    headPortrait = commentUser?.Avatar,
                    latelyLoginTime = comment.CommentTime.ToDateTimeString(),
                    registrationDate = comment.CommentTime.ToDateTimeString(),
                    nickname = commentUser.NikeName,
                    sex = commentUser.Sex,
                    userId = commentUser.SysUserId,//博主的userId='admin'
                    userType = commentUser.UserType
                },
            };
            var formartRepaly = new Reply
            {
                replyId = reply.SysCmsInfoCommentId,
                comment = formartReplyComment,
                content = reply.Comment,
                replyDate = reply.CommentTime.ToDateTimeString(),
                site = reply.Address,
                user = new CommentUser
                {
                    commentNum = 0,
                    headPortrait = replyUser?.Avatar,
                    latelyLoginTime = reply.CommentTime.ToDateTimeString(),
                    registrationDate = reply.CommentTime.ToDateTimeString(),
                    nickname = replyUser.NikeName,
                    sex = replyUser.Sex,
                    userId = replyUser.SysUserId,//博主的userId='admin'
                    userType = replyUser.UserType
                },
            };
            return formartRepaly;
        }
    }
}
