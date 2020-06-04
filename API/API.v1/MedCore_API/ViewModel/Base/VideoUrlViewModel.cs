using MedCoreAPI.ViewModel.Base;

namespace MedCore_API.ViewModel.Base
{
    public class VideoUrlViewModel
    {
        public string Url { get; set; }

        public int UltimaPosicaoAluno { get; set; }

        public VideoQualidadeViewModel[] Links { get; set; }
    }
}