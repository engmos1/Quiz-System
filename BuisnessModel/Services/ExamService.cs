using AutoMapper;
using BuisnessModel.DTOs.Exam;
using BuisnessModel.Interfaces;
using ExaminationSystem.Models;

public class ExamService
{
    private readonly IMapper _mapper;
    private readonly IExamRepository _examRepository;

    public ExamService(IMapper mapper, IExamRepository examRepository)
    {
        _mapper = mapper;
        _examRepository = examRepository;
    }

    public async Task<List<AllExamsDTO>> GetAll()
    {
        var exams = _examRepository.GetAll();
        return _mapper.ProjectTo<AllExamsDTO>(exams).ToList();
    }

    public async Task<ExamsDTO?> GetById(int id)
    {
        if (id <= 0)
            return null;

        var exam = await _examRepository.GetByID(id);
        if (exam == null)
            return null;

        return _mapper.Map<ExamsDTO>(exam);
    }

    public async Task<bool> AddExam(ExamsDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            return false;

        var entity = _mapper.Map<Exam>(dto);
        await _examRepository.Add(entity);

        return true;
    }

    public async Task<bool> UpdateExam(ExamsDTO dto)
    {
        if (dto.ID <= 0)
            return false;

        var existing = await _examRepository.GetByID(dto.ID);
        if (existing == null)
            return false;

        var entity = _mapper.Map<Exam>(dto);
        await _examRepository.Update(entity);
        return true;
    }

    public async Task<bool> DeleteExam(int id)
    {
        if (id <= 0)
            return false;

        var exam = await _examRepository.GetByID(id);
        if (exam == null)
            return false;

        await _examRepository.Delete(id);
        return true;
    }
}
