using System.Threading.Tasks;
using CoreDAL;
using CoreLogic;
using CoreLogic.Dto;
using CoreService.Dto;
using CoreWebCommon.Dto;
using ExpectedObjects;
using NUnit.Framework;

namespace CoreLogicTests
{
    [TestFixture]
    public class BoardLogicTests
    {
        private const int DefaultPageSize = 10;
        private FakeBoardLogic _boardLogic;
        private readonly SearchParamDto _defaultSearchParam = new SearchParamDto();

        [SetUp]
        public void Setup()
        {
            _boardLogic = new FakeBoardLogic(new Operation());
        }

        [Test]
        public async Task board_api_has_error()
        {
            PresetBoardApiRespFail();
            var boardList = await _boardLogic.GetBoardList(_defaultSearchParam, DefaultPageSize);
            ResultShouldBe(boardList, false, "Error");
        }

        private static void ResultShouldBe(IsSuccessResult<BoardListDto> boardList, bool isSuccess, string errorMessage)
        {
            var expect = new IsSuccessResult<BoardListDto>()
            {
                IsSuccess = isSuccess,
                ErrorMessage = errorMessage
            };
            boardList.ToExpectedObject().ShouldMatch(expect);
        }

        private void PresetBoardApiRespFail()
        {
            _boardLogic.SetBoardQueryResp(new BoardQueryResp()
            {
                IsSuccess = false
            });
        }

        private class FakeBoardLogic : BoardLogic
        {
            private BoardQueryResp _boardQueryResp;

            public FakeBoardLogic(Operation operation, BoardDa da = null) : base(operation, da)
            {
            }

            public void SetBoardQueryResp(BoardQueryResp boardQueryResp)
            {
                _boardQueryResp = boardQueryResp;
            }

            protected override Task<BoardQueryResp> BoardQueryResp(BoardQueryDto queryDto)
            {
                return Task.FromResult(_boardQueryResp);
            }
        }
    }
}