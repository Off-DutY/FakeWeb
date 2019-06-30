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
            PresetBoardQueryRespFail();
            var boardList = await _boardLogic.GetBoardList(_defaultSearchParam, DefaultPageSize);
            ShouldFalseAndHaveErrorMessage(boardList);
        }

        private static void ShouldFalseAndHaveErrorMessage(IsSuccessResult<BoardListDto> boardList)
        {
            var expect = new IsSuccessResult<BoardListDto>()
            {
                IsSuccess = false,
                ErrorMessage = "Error"
            };
            boardList.ToExpectedObject().ShouldMatch(expect);
        }

        private void PresetBoardQueryRespFail()
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